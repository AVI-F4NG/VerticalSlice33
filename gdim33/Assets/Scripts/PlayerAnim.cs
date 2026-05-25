using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerAnim : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Animator animator;
    [SerializeField] private Transform watchedTransform; // The transform that actually moves

    [Header("Animation State Names")]
    [SerializeField] private string idleState = "idle";
    [SerializeField] private string walkUpState = "walkup";
    [SerializeField] private string walkDownState = "walkdown";
    [SerializeField] private string walkLeftState = "walkleft";
    [SerializeField] private string walkRightState = "walkright";

    [Header("Tuning")]
    [SerializeField] private float moveThreshold = 0.000001f;
    [SerializeField] private float guardedSwitchDelay = 0.06f;

    private Vector3 lastPosition;

    private int currentStateHash;
    private int lastStableWalkHash;
    private int pendingStateHash;
    private float pendingStateStartTime;

    private int collisionContacts;

    private int idleHash;
    private int walkUpHash;
    private int walkDownHash;
    private int walkLeftHash;
    private int walkRightHash;

    private void Reset()
    {
        animator = GetComponent<Animator>();
        watchedTransform = transform;
    }

    private void Awake()
    {
        if (animator == null)
            animator = GetComponent<Animator>();

        if (watchedTransform == null)
            watchedTransform = transform;

        idleHash = Animator.StringToHash("Base Layer." + idleState);
        walkUpHash = Animator.StringToHash("Base Layer." + walkUpState);
        walkDownHash = Animator.StringToHash("Base Layer." + walkDownState);
        walkLeftHash = Animator.StringToHash("Base Layer." + walkLeftState);
        walkRightHash = Animator.StringToHash("Base Layer." + walkRightState);

        currentStateHash = idleHash;
        lastStableWalkHash = walkDownHash;

        lastPosition = watchedTransform.position;

        animator.Play(currentStateHash, 0, 0f);
    }

    private void LateUpdate()
    {
        Vector3 delta3 = watchedTransform.position - lastPosition;
        Vector2 delta = new Vector2(delta3.x, delta3.y);

        UpdateAnimation(delta);

        lastPosition = watchedTransform.position;
    }

    private void UpdateAnimation(Vector2 delta)
    {
        float ax = Mathf.Abs(delta.x);
        float ay = Mathf.Abs(delta.y);

        // No movement
        if (delta.sqrMagnitude <= moveThreshold)
        {
            pendingStateHash = 0;
            PlayIfChanged(idleHash);
            return;
        }

        bool diagonalMovement = ax > moveThreshold && ay > moveThreshold;
        bool guardedMode = diagonalMovement || collisionContacts > 0;

        int candidateState = GetCandidateState(delta, ax, ay);

        // Immediate switch for ordinary single-axis movement with no collision
        if (!guardedMode)
        {
            pendingStateHash = 0;
            PlayIfChanged(candidateState);
            lastStableWalkHash = candidateState;
            return;
        }

        // In guarded mode, do not flicker between directions.
        // If exactly diagonal or nearly equal, keep previous stable facing.
        if (diagonalMovement && Mathf.Abs(ax - ay) <= moveThreshold * 10f)
        {
            candidateState = lastStableWalkHash;
        }

        if (candidateState == currentStateHash)
        {
            pendingStateHash = 0;
            return;
        }

        if (pendingStateHash != candidateState)
        {
            pendingStateHash = candidateState;
            pendingStateStartTime = Time.time;
            return;
        }

        if (Time.time - pendingStateStartTime >= guardedSwitchDelay)
        {
            PlayIfChanged(candidateState);
            lastStableWalkHash = candidateState;
            pendingStateHash = 0;
        }
    }

    private int GetCandidateState(Vector2 delta, float ax, float ay)
    {
        if (ax > ay)
            return delta.x > 0f ? walkRightHash : walkLeftHash;

        if (ay > ax)
            return delta.y > 0f ? walkUpHash : walkDownHash;

        // Exact tie: keep the previous stable walk direction
        return lastStableWalkHash;
    }

    private void PlayIfChanged(int newStateHash)
    {
        if (currentStateHash == newStateHash)
            return;

        animator.Play(newStateHash, 0, 0f);
        currentStateHash = newStateHash;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        collisionContacts++;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        collisionContacts = Mathf.Max(0, collisionContacts - 1);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        collisionContacts++;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        collisionContacts = Mathf.Max(0, collisionContacts - 1);
    }
}