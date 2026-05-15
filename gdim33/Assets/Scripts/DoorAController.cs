using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorAController : MonoBehaviour
{
    [SerializeField] private DoorAPasswordUI passwordUI;
    [SerializeField] private GameObject lockedVisual;
    [SerializeField] private Collider2D blockingCollider;
    [SerializeField] private string nextSceneName = "Room3";

    private void Start()
    {
        RefreshLockedState();
    }

    public void Interact()
    {
        Debug.Log($"DoorA Interact: GameSession.doorAUnlocked={GameSession.doorAUnlocked}");

        if (GameSession.doorAUnlocked)
        {
            SceneManager.LoadScene(nextSceneName);
            return;
        }

        if (passwordUI != null)
            passwordUI.OpenPanel();
    }

    public void UnlockDoorSilently()
    {
        if (blockingCollider != null)
            blockingCollider.enabled = false;

        if (lockedVisual != null)
            lockedVisual.SetActive(false);
    }

    private void RefreshLockedState()
    {
        if (GameSession.doorAUnlocked)
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