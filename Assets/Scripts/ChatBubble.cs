using TMPro;
using UnityEngine;

public class ChatBubble : MonoBehaviour
{
    // The gameObject that has the input field component
    [SerializeField]
    private GameObject _textFieldObject;
    private TMP_Text _textField;
    
    public void SetBubbleText(string text)
    {
        _textFieldObject.GetComponent<TMP_Text>().text = text;
    }

    public TMP_Text GetBubbleTextField()
    {
        return _textFieldObject.GetComponent<TMP_Text>();
    }
}
