using UnityEngine;
using UnityEngine.SceneManagement;

public class EscapeToScene : MonoBehaviour
{
    [SerializeField] private string sceneName = "Room2";
    [SerializeField] private KeyCode triggerKey = KeyCode.Escape;

    private void Update()
    {
        if (Input.GetKeyDown(triggerKey))
        {
            SceneManager.LoadScene(sceneName);
        }
    }
}