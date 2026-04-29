using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneButton : MonoBehaviour
{
    [SerializeField] private string targetSceneName;

    public void LoadTargetScene()
    {
        Debug.Log($"CLICK FIRED -> loading: {targetSceneName}", this);
        if (string.IsNullOrWhiteSpace(targetSceneName))
        {
            Debug.LogError("Target scene name is empty.", this);
            return;
        }

        SceneManager.LoadScene(targetSceneName);
    }
}