using UnityEngine;

public class KeyCard2Pickup : MonoBehaviour
{
    [TextArea]
    [SerializeField] private string pickupMessage = "A higher-clearance key card.";

    private void Start()
    {
        if (GameSession.hasKeyCard2)
            gameObject.SetActive(false);
    }

    public void Interact()
    {
        if (GameSession.hasKeyCard2)
        {
            gameObject.SetActive(false);
            return;
        }

        GameSession.hasKeyCard2 = true;
        GameSession.hasFinalClearance = true;

        if (LocalHUD.Instance != null)
        {
            LocalHUD.Instance.SetKeyCard2Visible(true);
            LocalHUD.Instance.ShowMessage(pickupMessage, 3f);
        }

        gameObject.SetActive(false);
    }
}