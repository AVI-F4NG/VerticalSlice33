using UnityEngine;
using UnityEngine.SceneManagement;

public class GameProgress : MonoBehaviour
{
    public static GameProgress Instance { get; private set; }

    [Header("Progress Flags")]
    public bool usbFound;
    public bool doorAUnlocked;
    public bool hasFinalClearance;
    public bool computerLockedAfterUSBUse;

    [Header("References")]
    [SerializeField] private ProgressHUD hud;

    private void Awake()
    {
        Debug.Log($"GameProgress Awake on {gameObject.name}, instanceID={GetInstanceID()}, scene={gameObject.scene.name}");

        if (Instance != null && Instance != this)
        {
            Debug.Log($"Destroying duplicate GameProgress on {gameObject.name}, instanceID={GetInstanceID()}, scene={gameObject.scene.name}");
            Destroy(gameObject);
            return;
        }

        Instance = this;
        gameObject.name = "PersistentSystems [LIVE]";

        if (hud == null)
            hud = GetComponentInChildren<ProgressHUD>(true);

        DontDestroyOnLoad(gameObject);
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        if (Instance == this)
            SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void Start()
    {
        RefreshHUD();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (hud == null)
            hud = GetComponentInChildren<ProgressHUD>(true);

        RefreshHUD();
    }

    private void OnValidate()
    {
        if (hud == null)
            hud = GetComponentInChildren<ProgressHUD>(true);

        if (hud != null)
            hud.SetUSBVisible(usbFound);
    }

    public void SetUSBFound()
    {
        usbFound = true;
        RefreshHUD();
    }

    public void UnlockDoorA()
    {
        doorAUnlocked = true;
    }

    public void SetFinalClearance(bool value)
    {
        hasFinalClearance = value;
    }

    public void LockComputerAfterUSBUse()
    {
        computerLockedAfterUSBUse = true;
    }

    public void ShowMessage(string message, float seconds)
    {
        if (hud != null)
            hud.ShowMessage(message, seconds);
    }

    public void RefreshHUD()
    {
        if (hud != null)
            hud.SetUSBVisible(usbFound);
    }
}