using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;

public sealed class PersistentPlayerSceneBinder : MonoBehaviour
{
    [Header("Only bind in this scene")]
    [SerializeField] private string room1SceneName = "Room1";

    [Header("Tags in Room1")]
    [SerializeField] private string keyCardTag = "KeyCard1";
    [SerializeField] private string doorClosedTag = "Door1Closed";
    [SerializeField] private string doorTriggerTag = "Door1Trigger";

    [Header("Visual Scripting object variable names on Player")]
    [SerializeField] private string keyCardVariableName = "keyCard1Obj";
    [SerializeField] private string doorClosedVariableName = "door1ClosedObj";
    [SerializeField] private string doorTriggerVariableName = "door1TriggerObj";

    private VariableDeclarations objectVars;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        objectVars = Variables.Object(gameObject);
    }

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
        BindForScene(SceneManager.GetActiveScene());
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        BindForScene(scene);
    }

    private void BindForScene(Scene scene)
    {
        if (objectVars == null)
            objectVars = Variables.Object(gameObject);

        // Clear old references first so stale targets don't survive
        objectVars.Set(keyCardVariableName, null);
        objectVars.Set(doorClosedVariableName, null);
        objectVars.Set(doorTriggerVariableName, null);

        if (scene.name != room1SceneName)
        {
            Debug.Log($"PersistentPlayerSceneBinder: cleared Room1 refs for scene {scene.name}");
            return;
        }

        GameObject keyCard = FindSingleByTag(keyCardTag);
        GameObject doorClosed = FindSingleByTag(doorClosedTag);
        GameObject doorTrigger = FindSingleByTag(doorTriggerTag);

        objectVars.Set(keyCardVariableName, keyCard);
        objectVars.Set(doorClosedVariableName, doorClosed);
        objectVars.Set(doorTriggerVariableName, doorTrigger);

        Debug.Log(
            $"PersistentPlayerSceneBinder: rebound refs in {scene.name} | " +
            $"keyCard={(keyCard != null ? keyCard.name : "NULL")} | " +
            $"doorClosed={(doorClosed != null ? doorClosed.name : "NULL")} | " +
            $"doorTrigger={(doorTrigger != null ? doorTrigger.name : "NULL")}"
        );
    }

    private GameObject FindSingleByTag(string tagName)
    {
        GameObject[] matches = GameObject.FindGameObjectsWithTag(tagName);

        if (matches.Length == 0)
        {
            Debug.LogWarning($"PersistentPlayerSceneBinder: no object found with tag {tagName}");
            return null;
        }

        if (matches.Length > 1)
        {
            Debug.LogWarning(
                $"PersistentPlayerSceneBinder: multiple objects found with tag {tagName}, using {matches[0].name}"
            );
        }

        return matches[0];
    }
}