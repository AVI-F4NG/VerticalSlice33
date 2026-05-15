using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorBController : MonoBehaviour
{
    [Header("Locked Door")]
    [SerializeField] private GameObject lockedVisual;
    [SerializeField] private Collider2D blockingCollider;

    [Header("Success Action")]
    [SerializeField] private bool loadNextSceneOnUse = true;
    [SerializeField] private string nextSceneName = "Ending";

    [Header("Failure Message")]
    [SerializeField] private string lockedMessage = "Requires higher clearance key";
    [SerializeField] private float messageDuration = 3f;

    private void Start()
    {
        RefreshState();
    }

    public void Interact()
    {
        if (!GameSession.hasKeyCard2)
        {
            if (LocalHUD.Instance != null)
                LocalHUD.Instance.ShowMessage(lockedMessage, messageDuration);

            return;
        }

        UnlockDoorSilently();

        if (loadNextSceneOnUse && !string.IsNullOrEmpty(nextSceneName))
            SceneManager.LoadScene(nextSceneName);
    }

    public void UnlockDoorSilently()
    {
        if (blockingCollider != null)
            blockingCollider.enabled = false;

        if (lockedVisual != null)
            lockedVisual.SetActive(false);
    }

    private void RefreshState()
    {
        if (GameSession.hasKeyCard2)
        {
            UnlockDoorSilently();
            return;
        }

        if (blockingCollider != null)
            blockingCollider.enabled = true;

        if (lockedVisual != null)
            lockedVisual.SetActive(true);
    }
}