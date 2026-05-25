using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneCollision : MonoBehaviour
{
    [SerializeField] private string SceneName;
    [SerializeField] private bool canLoad;

    private void Update() { }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!canLoad)
            return;

        if (collision.gameObject.CompareTag("Player"))
        {
            SceneManager.LoadScene(SceneName);
        }
    }

    public void SetCanLoad(bool value)
    {
        canLoad = value;
    }
}