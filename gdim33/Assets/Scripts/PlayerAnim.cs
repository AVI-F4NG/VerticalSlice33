using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerAnim : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private float idleThreshold = 0.000001f;

    private Vector3 lastPosition;
    private string currentState = "";

    private void Awake()
    {
        if (animator == null)
            animator = GetComponent<Animator>();

        lastPosition = transform.position;
    }

    private void LateUpdate()
    {
        Vector3 delta = transform.position - lastPosition;

        string targetState;

        if (delta.sqrMagnitude <= idleThreshold)
        {
            targetState = "Base Layer.idle";
        }
        else if (Mathf.Abs(delta.x) > Mathf.Abs(delta.y))
        {
            targetState = delta.x > 0f
                ? "Base Layer.walkright"
                : "Base Layer.walkleft";
        }
        else
        {
            targetState = delta.y > 0f
                ? "Base Layer.walkup"
                : "Base Layer.walkdown";
        }

        if (currentState != targetState)
        {
            animator.Play(targetState, 0, 0f);
            currentState = targetState;
        }

        lastPosition = transform.position;
    }
}