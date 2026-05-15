using UnityEngine;

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
        if (player == null)
        {
            GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
            if (playerObject != null)
            {
                player = playerObject.transform;
            }
        }

        if (uiCanvasRoot != null)
        {
            uiCanvasRoot.SetActive(false);
        }
    }

    private void Update()
    {
        if (player == null || uiCanvasRoot == null)
        {
            return;
        }

        if (isUIOpen)
        {
            if (Input.GetKeyDown(closeKey))
            {
                CloseUI();
            }

            return;
        }

        bool isPlayerInRange = Vector2.Distance(player.position, transform.position) <= interactionRadius;

        if (isPlayerInRange && !UIKeyboardLock.IsLocked && Input.GetKeyDown(interactKey))
        {
            OpenUI();
        }
    }

    private void OpenUI()
    {
        isUIOpen = true;

        if (computerUI != null)
        {
            computerUI.OpenScreen();
        }
        else
        {
            uiCanvasRoot.SetActive(true);
        }

        UIKeyboardLock.Lock();
    }

    private void CloseUI()
    {
        isUIOpen = false;

        if (computerUI != null)
        {
            computerUI.CloseImmediate();
        }
        else
        {
            uiCanvasRoot.SetActive(false);
        }

        UIKeyboardLock.Unlock();
    }

    private void OnDisable()
    {
        if (isUIOpen)
        {
            CloseUI();
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactionRadius);
    }
}