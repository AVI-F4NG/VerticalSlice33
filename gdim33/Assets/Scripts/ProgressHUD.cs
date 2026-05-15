using System.Collections;
using TMPro;
using UnityEngine;

public class ProgressHUD : MonoBehaviour
{
    [SerializeField] private GameObject usbIcon;
    [SerializeField] private GameObject messageRoot;
    [SerializeField] private TMP_Text messageText;

    private Coroutine messageRoutine;

    private void Awake()
    {
        if (messageRoot != null)
            messageRoot.SetActive(false);
    }

    public void SetUSBVisible(bool visible)
    {
        if (usbIcon != null)
            usbIcon.SetActive(visible);
    }

    public void ShowMessage(string message, float seconds)
    {
        if (messageRoutine != null)
            StopCoroutine(messageRoutine);

        messageRoutine = StartCoroutine(ShowMessageRoutine(message, seconds));
    }

    private IEnumerator ShowMessageRoutine(string message, float seconds)
    {
        if (messageText != null)
            messageText.text = message;

        if (messageRoot != null)
            messageRoot.SetActive(true);

        yield return new WaitForSeconds(seconds);

        if (messageRoot != null)
            messageRoot.SetActive(false);

        messageRoutine = null;
    }
}