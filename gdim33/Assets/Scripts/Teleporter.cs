using UnityEngine;

public class Teleporter : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform player;
    [SerializeField] private Transform destinationPoint;

    [Header("Interaction")]
    [SerializeField] private float interactionRadius = 2f;
    [SerializeField] private KeyCode interactKey = KeyCode.E;
    [SerializeField] private string promptMessage = "What kind of device is this? Should I try interacting with it?";

    private Rigidbody2D playerRb;
    private bool wasInRange;

    private void Awake()
    {
        if (player == null)
        {
            GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
            if (playerObject != null)
                player = playerObject.transform;
        }

        if (player != null)
            playerRb = player.GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (player == null || destinationPoint == null)
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

        if (inRange && Input.GetKeyDown(interactKey)){
            SFXManager.PlayBeep();
            TeleportPlayer();
        }

        wasInRange = inRange;
    }

    private void TeleportPlayer()
    {
        if (playerRb != null)
        {
            playerRb.velocity = Vector2.zero;
            playerRb.position = destinationPoint.position;
        }
        else
        {
            player.position = destinationPoint.position;
        }

        if (LocalHUD.Instance != null)
            LocalHUD.Instance.HideMessage();

        wasInRange = false;
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

        if (destinationPoint != null)
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawLine(transform.position, destinationPoint.position);
            Gizmos.DrawSphere(destinationPoint.position, 0.15f);
        }
    }
}