using UnityEngine;

public class USBPickup : MonoBehaviour
{
    [TextArea]
    [SerializeField] private string pickupMessage = "A USB stick, maybe I can try using it on the computer?";

    public void Interact()
    {
        Debug.Log("USBPickup.Interact called.");

        if (GameSession.usbFound)
        {
            Debug.Log("USB already found; skipping pickup.");
            return;
        }

        GameSession.usbFound = true;
        Debug.Log($"GameSession.usbFound set to {GameSession.usbFound}");

        if (LocalHUD.Instance == null)
        {
            Debug.LogError("LocalHUD.Instance is NULL. No active LocalHUD in this scene.");
        }
        else
        {
            Debug.Log("LocalHUD.Instance found. Updating HUD.");
            LocalHUD.Instance.SetUSBVisible(true);
            LocalHUD.Instance.ShowMessage(pickupMessage, 3f);
        }

        gameObject.SetActive(false);
    }
}