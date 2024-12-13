using UnityEngine;
using UnityEngine.UI;

public class InteractablePrompt : MonoBehaviour, IInteractable
{
    private Outline Outline;
    private Canvas Canvas;

    void Start()
    {
        // Ensure the object has an Outline component
        Outline = GetComponent<Outline>();
        if (Outline != null)
        {
            Outline.enabled = false; // Disable the outline initially
        }

        Canvas = FindAnyObjectByType<Canvas>();
        if (Canvas)
        {
            Canvas.gameObject.SetActive(false);
        }
    }

    public void Interact()
    {
        Canvas.gameObject.SetActive(true);
    }

    public void SetHighlight(bool bState)
    {
        if (Outline)
        {
            Outline.enabled = bState;
        }
    }

    public void StopInteract()
    {
        Canvas.gameObject.SetActive(false);
    }
}
