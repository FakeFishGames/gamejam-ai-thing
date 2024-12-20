using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScreen : MonoBehaviour
{
    public GameObject[] objectsToEnable;

    private int currentIndex = 0;

    public float delay;

    private float timer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (Input.anyKey || (timer > delay && currentIndex < objectsToEnable.Length))
        {
            if (currentIndex >= objectsToEnable.Length)
            {
                SceneManager.LoadScene(1);
            }
            else
            {
                objectsToEnable[currentIndex].SetActive(true);
                currentIndex++;
            }
            timer = 0.0f;
        }
    }
}
