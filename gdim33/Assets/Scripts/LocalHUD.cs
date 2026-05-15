using System.Collections;
using TMPro;
using UnityEngine;

public sealed class LocalHUD : MonoBehaviour
{
    public static LocalHUD Instance { get; private set; }

    [SerializeField] private GameObject usbIcon;
    [SerializeField] private GameObject keyCard2Icon;
    [SerializeField] private GameObject messageRoot;
    [SerializeField] private TMP_Text messageText;

    private Coroutine messageRoutine;

    private void Awake()
    {
        Instance = this;

        if (messageRoot != null)
            messageRoot.SetActive(false);
    }

    private void Start()
    {
        SetUSBVisible(GameSession.usbFound);
        SetKeyCard2Visible(GameSession.hasKeyCard2);
    }

    private void OnDestroy()
    {
        if (Instance == this)
            Instance = null;
    }

    public void SetUSBVisible(bool visible)
    {
        if (usbIcon != null)
            usbIcon.SetActive(visible);
    }

    public void SetKeyCard2Visible(bool visible)
    {
        if (keyCard2Icon != null)
            keyCard2Icon.SetActive(visible);
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