using UnityEngine;
using System.Collections.Generic;
using System.Threading.Tasks;
using LLMUnity;
using StarterAssets;
using TMPro;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Button = UnityEngine.UI.Button;
using Cursor = UnityEngine.UIElements.Cursor;
using NotImplementedException = System.NotImplementedException;

namespace LLMUnitySamples
{
    public class ChatBotWithDirector : MonoBehaviour
    {
        public Transform chatContainer;
        public Color playerColor = new Color32(81, 164, 81, 255);
        public Color aiColor = new Color32(29, 29, 73, 255);
        public Color fontColor = Color.white;
        public Font font;
        public int fontSize = 16;
        public int bubbleWidth = 600;
        public LLMCharacter llmCharacter;
        public LLMCharacter llmDirectorCharacter;
        public LLMDirector llmDirector;
        public float textPadding = 10f;
        public float bubbleSpacing = 10f;
        public Sprite sprite;
        public Button stopButton;

        private InputBubble inputBubble;
        private List<Bubble> chatBubbles = new List<Bubble>();
        private bool blockInput = true;
        private BubbleUI playerUI, aiUI;
        private bool warmUpDone = false;
        private int lastBubbleOutsideFOV = -1;

        public FirstPersonController firstPersonController;

        // Different UI objects
        [SerializeField]
        private GameObject _playerInputObject;
        private TMP_InputField _playerInputField;

        [SerializeField]
        private GameObject _chatScrollObject;
        private ScrollRect _chatScrollRect;

        [SerializeField]
        private GameObject _chatContainerObject;
        private VerticalLayoutGroup _chatContainerVBox;

        [SerializeField]
        private GameObject _chatMessageObject_Player;
        [SerializeField]
        private GameObject _chatMessageObject_Monster;

        void AddBubbleToChat(GameObject bubbleObject)
        {
            bubbleObject.transform.SetParent(_chatContainerObject.transform, false);
        }
        
        void OnPlayerInputSubmitted(TMP_InputField inputField)
        {
            BlockInteraction();
            
            // Create two new chat bubbles after a message has been submitted, one for the player and one for the AI to write in
            GameObject playerMessageBubble = Instantiate(_chatMessageObject_Player);
            playerMessageBubble.GetComponent<ChatBubble>().SetBubbleText(inputField.text);
            AddBubbleToChat(playerMessageBubble);

            GameObject aiMessageBubble = Instantiate(_chatMessageObject_Monster);
            var textField = aiMessageBubble.GetComponent<ChatBubble>().GetBubbleTextField();
            AddBubbleToChat(aiMessageBubble);
            
            llmDirector.ReceivePlayerMessage(inputField.text);
            llmDirector.StartAICharacterMessage();
            Task chatTask = llmCharacter.Chat(inputField.text, (string text) =>
            {
                textField.text = text;
                llmCharacterMessage = text;
            }, completionCallback: () =>
            {
                AllowInput();
                llmDirector.ReceiveAICharacterMessage(llmCharacterMessage);
            });
            
            // Clear the player input text field
            inputField.text = "";
        }
        
        void Start()
        {
            // Force cursor always on
            UnityEngine.Cursor.lockState = CursorLockMode.None;
            UnityEngine.Cursor.visible = true;
            
            // UI setup
            _playerInputField = _playerInputObject.GetComponent<TMP_InputField>();
            _playerInputField.onEndEdit.AddListener(delegate{OnPlayerInputSubmitted(_playerInputField);});
            
            _chatScrollRect = _chatScrollObject.GetComponent<ScrollRect>();
            _chatContainerVBox = _chatContainerObject.GetComponent<VerticalLayoutGroup>();
            
            // inputBubble = new InputBubble(chatContainer, playerUI, "InputBubble", "Loading...", 4);
            // inputBubble.AddSubmitListener(onInputFieldSubmit);
            // inputBubble.AddValueChangedListener(onValueChanged);
            // inputBubble.setInteractable(false);
            // stopButton.gameObject.SetActive(true);
            _ = llmCharacter.Warmup(WarmUpCallback);
            _ = llmDirectorCharacter.Warmup(WarmUpCallback);
        }

        void onInputFieldSubmit(string newText)
        {
            //inputBubble.ActivateInputField();
            if (blockInput || newText.Trim() == "" || Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
            {
                StartCoroutine(BlockInteraction());
                return;
            }
            blockInput = true;
            // replace vertical_tab
            string message = inputBubble.GetText().Replace("\v", "\n");

            Bubble playerBubble = new Bubble(chatContainer, playerUI, "PlayerBubble", message);
            Bubble aiBubble = new Bubble(chatContainer, aiUI, "AIBubble", "...");
            chatBubbles.Add(playerBubble);
            chatBubbles.Add(aiBubble);
            playerBubble.OnResize(UpdateBubblePositions);
            aiBubble.OnResize(UpdateBubblePositions);

            llmDirector.ReceivePlayerMessage(message);

            llmDirector.StartAICharacterMessage();
            Task chatTask = llmCharacter.Chat(message, (string text) =>
            {
                aiBubble.SetText(text);
                llmCharacterMessage = text;
            }, completionCallback: () =>
            {
                AllowInput();
                llmDirector.ReceiveAICharacterMessage(llmCharacterMessage);
            });

            inputBubble.SetText("");
        }

        private string llmCharacterMessage, llmDirectorMessageFromPlayer, llmDirectorMessageFromAICharacter;

        public void WarmUpCallback()
        {
            warmUpDone = true;
            AllowInput();
        }

        public void AllowInput()
        {
            blockInput = false;
            _playerInputField.interactable = true;
        }

        IEnumerator<string> BlockInteraction()
        {
            // prevent from change until next frame
            _playerInputField.interactable = false;
            yield return null;
            _playerInputField.interactable = true;
            // change the caret position to the end of the text
            _playerInputField.MoveTextEnd(true);
        }

        void onValueChanged(string newText)
        {
            // Get rid of newline character added when we press enter
            if (Input.GetKey(KeyCode.Return))
            {
                if (inputBubble.GetText().Trim() == "")
                    inputBubble.SetText("");
            }
        }

        public void UpdateBubblePositions()
        {
            float y = inputBubble.GetSize().y + inputBubble.GetRectTransform().offsetMin.y + bubbleSpacing;
            float containerHeight = chatContainer.GetComponent<RectTransform>().rect.height;
            for (int i = chatBubbles.Count - 1; i >= 0; i--)
            {
                Bubble bubble = chatBubbles[i];
                RectTransform childRect = bubble.GetRectTransform();
                childRect.anchoredPosition = new Vector2(childRect.anchoredPosition.x, y);

                // last bubble outside the container
                if (y > containerHeight && lastBubbleOutsideFOV == -1)
                {
                    lastBubbleOutsideFOV = i;
                }
                y += bubble.GetSize().y + bubbleSpacing;
            }
        }

        void Update()
        {
            // if (!inputBubble.inputFocused() && warmUpDone)
            // {
                //inputBubble.ActivateInputField();
                //StartCoroutine(BlockInteraction());
            // }

            // if (firstPersonController != null)
            // {
            //     firstPersonController.enabled = !inputBubble.inputFocused();
            // }

            // inputBubble.inputFocused();
            
            // if (Input.GetKeyDown(KeyCode.Return))
            // {
            //     inputBubble.ActivateInputField();
            //     StartCoroutine(BlockInteraction());
            // }

            // if (lastBubbleOutsideFOV != -1)
            // {
            //     // destroy bubbles outside the container
            //     for (int i = 0; i <= lastBubbleOutsideFOV; i++)
            //     {
            //         chatBubbles[i].Destroy();
            //     }
            //     chatBubbles.RemoveRange(0, lastBubbleOutsideFOV + 1);
            //     lastBubbleOutsideFOV = -1;
            // }
        }

        public void ExitGame()
        {
            Debug.Log("Exit button clicked");
            Application.Quit();
        }

        bool onValidateWarning = true;
        void OnValidate()
        {
            if (onValidateWarning && !llmCharacter.remote && llmCharacter.llm != null && llmCharacter.llm.model == "")
            {
                Debug.LogWarning($"Please select a model in the {llmCharacter.llm.gameObject.name} GameObject!");
                onValidateWarning = false;
            }
            if (onValidateWarning && !llmDirectorCharacter.remote && llmDirectorCharacter.llm != null && llmDirectorCharacter.llm.model == "")
            {
                Debug.LogWarning($"Please select a model in the {llmDirectorCharacter.llm.gameObject.name} GameObject!");
                onValidateWarning = false;
            }
        }
    }
}
