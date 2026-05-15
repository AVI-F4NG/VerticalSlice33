using UnityEngine;

public sealed class ProximityInteractCall2D : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform player;
    [SerializeField] private MonoBehaviour interactTarget;

    [Header("Interaction")]
    [SerializeField] private float interactionRadius = 2f;
    [SerializeField] private KeyCode interactKey = KeyCode.E;

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
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactionRadius);
    }
}