using UnityEngine;

public class ToggleObjectsOnCtrl : MonoBehaviour
{
    [SerializeField]
    private GameObject[] toggleOnInteract;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl) || Input.GetKeyDown(KeyCode.RightControl))
        {
            foreach (var gameObject in toggleOnInteract)
            {
                gameObject.SetActive(!gameObject.activeSelf);
            }
        }
    }
}
