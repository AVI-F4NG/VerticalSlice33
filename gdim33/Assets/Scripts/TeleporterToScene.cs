using UnityEngine;
using UnityEngine.SceneManagement;

public class TeleporterToScene : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform player;

    [Header("Scene Loading")]
    [SerializeField] private string targetSceneName = "Room1";

    [Header("Interaction")]
    [SerializeField] private float interactionRadius = 2f;
    [SerializeField] private KeyCode interactKey = KeyCode.E;
    [SerializeField] private string promptMessage = "What kind of device is this? Should I try interacting with it?";

    private bool wasInRange;

    private void Awake()
    {
        if (player == null)
        {
            GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
            if (playerObject != null)
                player = playerObject.transform;
        }
    }

    private void Update()
    {
        if (player == null)
            return;

        bool inRange = Vector2.Distance(player.position, transform.position) <= interactionRadius;

        if (inRange && !wasInRange)
        {
            if (LocalHUD.Instance != null)
                LocalHUD.Instance.ShowPersistentMessage(promptMessage);
        }
        else if (!inRange && wasInRange)
        {
            if (LocalHUD.Instance != null)
                LocalHUD.Instance.HideMessage();
        }

        if (inRange && Input.GetKeyDown(interactKey))
        {
            if (LocalHUD.Instance != null)
                LocalHUD.Instance.HideMessage();

            SceneManager.LoadScene(targetSceneName);
        }

        wasInRange = inRange;
    }

    private void OnDisable()
    {
        if (wasInRange && LocalHUD.Instance != null)
            LocalHUD.Instance.HideMessage();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, interactionRadius);
    }
}