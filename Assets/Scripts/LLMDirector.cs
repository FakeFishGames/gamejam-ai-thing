using UnityEngine;
using LLMUnity;
using UnityEngine.SceneManagement;
using System.Collections;

public class LLMDirector : MonoBehaviour
{
    public LLMCharacter llmDirectorCharacter;

    public Animator aiCharacterAnimator;

    string llmDirectorMessage;

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
                    Debug.Log("The true name has been revealed, you won the game!");
                }
            });
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
        SceneManager.LoadScene(SceneManager.loadedSceneCount - 1);
    }
}
