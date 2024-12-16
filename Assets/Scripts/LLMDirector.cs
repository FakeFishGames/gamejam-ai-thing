using UnityEngine;
using LLMUnity;
using UnityEngine.SceneManagement;
using System.Collections;

public class LLMDirector : MonoBehaviour
{
    public LLMCharacter aiCharacter;
    public LLMCharacter llmDirectorCharacter;

    public Animator aiCharacterAnimator;

    string llmDirectorMessage;

    public GameObject objectThatCausesAggression;

    public string initialPrompt;

    public string promptOnAngry;

    public string promptOnHostile;

    private float angryToHostileTimer;

    public float AngryToHostileDelay = 30.0f;

    public void Start()
    {
        aiCharacter.SetPrompt(initialPrompt);
    }

    private void SetPromptIfNeeded(LLMCharacter character, string prompt)
    {
        if (prompt != character.prompt)
        {
            aiCharacter.SetPrompt(prompt);
        }
    }

    public void Update()
    {
        if (objectThatCausesAggression != null && objectThatCausesAggression.activeInHierarchy)
        {
            angryToHostileTimer += Time.deltaTime;
            if (angryToHostileTimer > AngryToHostileDelay)
            {
                SetPromptIfNeeded(aiCharacter, promptOnHostile);
            }
            else
            {
                SetPromptIfNeeded(aiCharacter, promptOnAngry);
            }
        }
        else
        {
            angryToHostileTimer = Mathf.Max(0, angryToHostileTimer - Time.deltaTime);
            SetPromptIfNeeded(aiCharacter, initialPrompt);
        }
    }

    public void ReceivePlayerMessage(string message)
    {
        Debug.Log("LLMDirector received player message: " + message);
    }

    public void StartAICharacterMessage()
    {
        aiCharacterAnimator.SetTrigger(Random.Range(0.0f, 1.0f) < 0.5f ? "Speak" : "Spasm");
    }

    public void ReceiveAICharacterMessage(string message)
    {
        Debug.Log("LLMDirector received AI character message: " + message);
        AskAIDirector(
            "If you find the phrase \"Tol Cormpt Norz Norz Norz\" in the following text, respond with \"yes\", if you don't, respond with \"no\"."+
            " Only respond with \"yes\" or \"no\".\n The text: "+ message,
            (string response) =>
            {
                if (response == "yes")
                {
                    EndGame();
                }
            });

        /*AskAIDirector(
            "If the speaker has expressed that they are going to kill or destroy whoever they are speaking to, answer \"yes\", if you don't, respond with \"no\"." +
            " Only respond with \"yes\" or \"no\".\n The text: " + message,
            (string response) =>
            {
                if (response == "yes")
                {
                    Debug.Log("Game over!");
                }
            });*/
    }

    /*public void ReceiveAIDirectorMessageInResponseToPlayerMessage(string message)
    {
        Debug.Log("LLMDirector received AI director message in response to player message: " + message);
    }

    public void ReceiveAIDirectorMessageInResponseToAICharacterMessage(string message)
    {
        Debug.Log("LLMDirector received AI director message in response to AI character message: " + message);
    }*/

    private void AskAIDirector(string message, Callback<string> callback = null)
    {
        //director receives the player input too
        _ = llmDirectorCharacter.Chat(message, (string text) =>
        {
            llmDirectorMessage = text;
        }, completionCallback: () =>
        {
            Debug.Log($"AI director response: {llmDirectorMessage}. Question: {message}");
            callback(llmDirectorMessage);
        });
    }

    public void EndGame()
    {
        aiCharacterAnimator.SetTrigger("Death");
        Debug.Log("The true name has been revealed, you won the game!");
        StartCoroutine(ExecuteAfterTime(0.5f));
    }

    IEnumerator ExecuteAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
        SceneManager.LoadScene(SceneManager.sceneCount - 1);
    }
}
