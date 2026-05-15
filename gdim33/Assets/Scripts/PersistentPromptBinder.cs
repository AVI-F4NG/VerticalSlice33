using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;

public sealed class PersistentPromptBinder : MonoBehaviour
{
    [Header("Persistent prompt lookup")]
    [SerializeField] private string promptTag = "InteractionPrompt";

    [Header("Player object variable name in Visual Scripting")]
    [SerializeField] private string promptVariableName = "interactionPromptObj";

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void Start()
    {
        BindPrompt();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        BindPrompt();
    }

    private void BindPrompt()
    {
        GameObject prompt = GameObject.FindGameObjectWithTag(promptTag);

        if (prompt == null)
        {
            Debug.LogWarning($"PersistentPromptBinder: no object found with tag {promptTag}");
            return;
        }

        Variables.Object(gameObject).Set(promptVariableName, prompt);
        Debug.Log($"PersistentPromptBinder: bound {prompt.name} to variable {promptVariableName}");
    }
}