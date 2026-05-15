using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public sealed class BootstrapLoader : MonoBehaviour
{
    [SerializeField] private string firstGameplayScene = "Room1";

    private IEnumerator Start()
    {
        yield return null;
        SceneManager.LoadScene(firstGameplayScene, LoadSceneMode.Single);
    }
}