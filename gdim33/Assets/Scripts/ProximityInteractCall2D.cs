using UnityEngine;

public sealed class ProximityInteractCall2D : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform player;
    [SerializeField] private MonoBehaviour interactTarget;

    [Header("Interaction")]
    [SerializeField] private float interactionRadius = 2f;
    [SerializeField] private KeyCode interactKey = KeyCode.E;

    [Header("Picture Interaction")]
    [SerializeField] private bool isPictureInteract;
    [SerializeField] private string pictureMessage = "This picture looks suspicious...";
    [SerializeField] private PictureReveal pictureReveal;
    

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
        if (player == null || interactTarget == null)
            return;

        bool isPlayerInRange = Vector2.Distance(player.position, transform.position) <= interactionRadius;

        if (isPlayerInRange && Input.GetKeyDown(interactKey))
        {
            interactTarget.SendMessage("Interact", SendMessageOptions.DontRequireReceiver);
        }

        if (isPlayerInRange && isPictureInteract && !pictureReveal.revealed)
        {
            if (LocalHUD.Instance != null)
            {
                LocalHUD.Instance.ShowMessage(pictureMessage, 3f);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactionRadius);
    }
}