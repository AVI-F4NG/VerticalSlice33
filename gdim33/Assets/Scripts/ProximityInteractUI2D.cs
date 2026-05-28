using UnityEngine;
using UnityEngine.SceneManagement;

public sealed class ProximityInteractUI2D : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform player;
    [SerializeField] private GameObject uiCanvasRoot;
    [SerializeField] private ComputerMonitorUI computerUI;

    [Header("Interaction")]
    [SerializeField] private float interactionRadius = 2f;
    [SerializeField] private KeyCode interactKey = KeyCode.E;
    [SerializeField] private KeyCode closeKey = KeyCode.Escape;

    private bool isUIOpen;

    private void Awake()
    {
        CachePlayer();

        if (uiCanvasRoot != null)
            uiCanvasRoot.SetActive(false);

        isUIOpen = false;
        UIKeyboardLock.Unlock();
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        ForceCloseAndReset();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        CachePlayer();
        ForceCloseAndReset();
    }

    private void Update()
    {
        if (player == null)
            CachePlayer();

        SyncOpenState();

        if (player == null)
            return;

        if (isUIOpen)
        {
            if (Input.GetKeyDown(closeKey))
                CloseUI();

            return;
        }

        bool isPlayerInRange = Vector2.Distance(player.position, transform.position) <= interactionRadius;

        if (!isPlayerInRange)
            return;

        // Only block interaction if another UI is really holding the lock.
        if (UIKeyboardLock.IsLocked)
            return;

        if (Input.GetKeyDown(interactKey))
            OpenUI();
    }

    private void CachePlayer()
    {
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
            player = playerObject.transform;
    }

    private void SyncOpenState()
    {
        if (computerUI != null)
        {
            isUIOpen = computerUI.IsOpen;
            return;
        }

        if (uiCanvasRoot != null)
            isUIOpen = uiCanvasRoot.activeInHierarchy;
        else
            isUIOpen = false;
    }

    private void OpenUI()
    {
        bool opened = false;

        if (computerUI != null)
        {
            opened = computerUI.OpenScreen();
        }
        else if (uiCanvasRoot != null)
        {
            uiCanvasRoot.SetActive(true);
            opened = uiCanvasRoot.activeInHierarchy;
        }

        isUIOpen = opened;

        if (opened)
            UIKeyboardLock.Lock();
    }

    public void CloseUI()
    {
        if (computerUI != null)
        {
            computerUI.CloseImmediate();
        }
        else if (uiCanvasRoot != null)
        {
            uiCanvasRoot.SetActive(false);
        }

        isUIOpen = false;
        UIKeyboardLock.Unlock();
    }

    private void ForceCloseAndReset()
    {
        if (computerUI != null)
            computerUI.CloseImmediate();
        else if (uiCanvasRoot != null)
            uiCanvasRoot.SetActive(false);

        isUIOpen = false;
        UIKeyboardLock.Unlock();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactionRadius);
    }
}