using UnityEngine;

public class InteractableObject : MonoBehaviour, IInteractable
{
    private Outline outline;
    [SerializeField] 
    private GameObject[] toggleOnInteract;

    void Start()
    {
        // Ensure the object has an Outline component
        outline = GetComponent<Outline>();
        if (outline != null)
        {
            outline.enabled = false; // Disable the outline initially
        }
    }

    public void Interact()
    {
        foreach (var gameObject in toggleOnInteract)
        {
            gameObject.SetActive(!gameObject.activeSelf);
        }
    }

    public void SetHighlight(bool bState)
    {
        if (outline)
        {
            outline.enabled = bState;
        }
    }
}
