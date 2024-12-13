using UnityEngine;

public interface IInteractable
{
    public void Interact();
    public void SetHighlight(bool bState);
    public void StopInteract();
}

public class Interactor : MonoBehaviour
{
    private float InteractRange = 3;
    public Transform InteractorSource;
    
    public Texture2D crosshairTexture;
    public Vector2 size = new Vector2(50, 50);

    private IInteractable Interactable;
    
    void Start()
    {

    }

    void Update()
    {
        Ray ray = new Ray(InteractorSource.position, InteractorSource.forward);
        if (Physics.Raycast(ray, out RaycastHit hitInfo, InteractRange))
        {
            if (hitInfo.collider.gameObject.TryGetComponent(out IInteractable interactObj))
            {
                Interactable = interactObj;
                if (Input.GetKeyDown(KeyCode.Return))
                {
                    Interactable.Interact();
                }
                
                Interactable.SetHighlight(true);
            }
            else if (Interactable != null)
            {
                InteractableRemoved();
            }
        }
        else if (Interactable != null)
        {
            InteractableRemoved();
        }

        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Tab))
        {
            if (Interactable != null)
            {
                Interactable.StopInteract();
            }
        }
    }

    void InteractableRemoved()
    {
        Interactable.SetHighlight(false);
        Interactable = null;
    }
    
    void OnGUI()
    {
        if (crosshairTexture != null)
        {
            // Calculate the center of the screen
            float x = (Screen.width - size.x) / 2;
            float y = (Screen.height - size.y) / 2;

            // Draw the crosshair texture
            GUI.DrawTexture(new Rect(x, y, size.x, size.y), crosshairTexture);
        }
        else
        {
            Debug.LogWarning("Crosshair texture not assigned!");
        }
    }
}
