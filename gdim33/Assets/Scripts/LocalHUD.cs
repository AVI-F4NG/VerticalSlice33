using System.Collections;
using TMPro;
using UnityEngine;

public sealed class LocalHUD : MonoBehaviour
{
    public static LocalHUD Instance { get; private set; }

    [SerializeField] private GameObject usbIcon;
    [SerializeField] private GameObject messageRoot;
    [SerializeField] private TMP_Text messageText;

    private Coroutine messageRoutine;

    private void Awake()
    {
        Instance = this;
        Debug.Log($"LocalHUD Awake on {gameObject.name}");

        if (messageRoot != null)
            messageRoot.SetActive(false);
        else
            Debug.LogWarning("LocalHUD: messageRoot is missing.");
    }

    private void Start()
    {
        Debug.Log($"LocalHUD Start. usbFound={GameSession.usbFound}");
        SetUSBVisible(GameSession.usbFound);
    }

    private void OnDestroy()
    {
        if (Instance == this)
            Instance = null;
    }

    public void SetUSBVisible(bool visible)
    {
        if (usbIcon == null)
        {
            Debug.LogError("LocalHUD: usbIcon is missing.");
            return;
        }

        Debug.Log($"LocalHUD.SetUSBVisible({visible}) on {usbIcon.name}");
        usbIcon.SetActive(visible);
        Debug.Log($"usbIcon activeSelf={usbIcon.activeSelf}, activeInHierarchy={usbIcon.activeInHierarchy}");
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
        else
            Debug.LogWarning("LocalHUD: messageText is missing.");

        if (messageRoot != null)
            messageRoot.SetActive(true);

        yield return new WaitForSeconds(seconds);

        if (messageRoot != null)
            messageRoot.SetActive(false);

        messageRoutine = null;
    }
}