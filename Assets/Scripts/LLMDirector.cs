using UnityEngine;

public class LLMDirector : MonoBehaviour
{
    public void ReceivePlayerMessage(string message)
    {
        Debug.Log("LLMDirector received player message: " + message);
    }

    public void ReceiveAICharacterMessage(string message)
    {
        Debug.Log("LLMDirector received AI character message: " + message);
    }

    public void ReceiveAIDirectorMessageInResponseToPlayerMessage(string message)
    {
        Debug.Log("LLMDirector received AI director message in response to player message: " + message);
    }

    public void ReceiveAIDirectorMessageInResponseToAICharacterMessage(string message)
    {
        Debug.Log("LLMDirector received AI director message in response to AI character message: " + message);
    }
}
