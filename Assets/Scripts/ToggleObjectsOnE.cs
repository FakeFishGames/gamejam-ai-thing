using UnityEngine;

public class ToggleObjectsOnE : MonoBehaviour
{
    [SerializeField]
    private GameObject[] toggleOnInteract;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            foreach (var gameObject in toggleOnInteract)
            {
                gameObject.SetActive(!gameObject.activeSelf);
            }
        }
    }
}
