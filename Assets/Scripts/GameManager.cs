using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Cursor = UnityEngine.Cursor;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private Canvas _uiCanvas;

    public VisualElement rootUIElement;
    public TextField PlayerTextInput;
    
    private void Start()
    {
        // Set the UI active on game start so we can put input there
        _uiCanvas.gameObject.SetActive(true);
        Cursor.visible = true;
    }

    private void OnEnable()
    {
        // PlayerTextInput = rootUIElement.Q<TextField>("PlayerTextInput");
        // PlayerTextInput.GetFirstOfType<InputField>()?.ActivateInputField();
        // Cursor.visible = true;
    }
}