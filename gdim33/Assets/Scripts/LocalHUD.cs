using System.Collections;
using TMPro;
using UnityEngine;

public sealed class LocalHUD : MonoBehaviour
{
    private static LocalHUD instance;
    public static LocalHUD Instance
    {
        get
        {
            if (instance == null)
                instance = FindFirstObjectByType<LocalHUD>();

            return instance;
        }
    }

    [SerializeField] private GameObject usbIcon;
    [SerializeField] private GameObject keyCard2Icon;
    [SerializeField] private GameObject messageRoot;
    [SerializeField] private TMP_Text messageText;

    private Coroutine messageRoutine;

    private void Awake()
    {
        instance = this;
        Debug.Log($"LocalHUD Awake on {gameObject.name}");

        if (messageRoot != null)
            messageRoot.SetActive(false);
        else
            Debug.LogWarning("LocalHUD: messageRoot is NULL");
    }

    private void OnEnable()
    {
        Debug.Log("LocalHUD OnEnable");
        RefreshProgressIcons();
    }

    private void Start()
    {
        Debug.Log($"LocalHUD Start | usbFound={GameSession.usbFound} | hasKeyCard2={GameSession.hasKeyCard2}");
        RefreshProgressIcons();
    }

    private void OnDestroy()
    {
        if (instance == this)
            instance = null;
    }

    public void RefreshProgressIcons()
    {
        SetUSBVisible(GameSession.usbFound);
        SetKeyCard2Visible(GameSession.hasKeyCard2);
    }

    public void SetUSBVisible(bool visible)
    {
        if (usbIcon == null)
        {
            Debug.LogWarning("LocalHUD: usbIcon is NULL");
            return;
        }

        usbIcon.SetActive(visible);
        Debug.Log($"LocalHUD SetUSBVisible({visible}) | activeInHierarchy={usbIcon.activeInHierarchy}");
    }

    public void SetKeyCard2Visible(bool visible)
    {
        if (keyCard2Icon == null)
        {
            Debug.LogWarning("LocalHUD: keyCard2Icon is NULL");
            return;
        }

        keyCard2Icon.SetActive(visible);
        Debug.Log($"LocalHUD SetKeyCard2Visible({visible}) | activeInHierarchy={keyCard2Icon.activeInHierarchy}");
    }

    public void ShowMessage(string message, float seconds)
    {
        Debug.Log($"LocalHUD ShowMessage: {message}");

        if (messageRoutine != null)
            StopCoroutine(messageRoutine);

        messageRoutine = StartCoroutine(ShowMessageRoutine(message, seconds));
    }

    public void ShowPersistentMessage(string message)
    {
        Debug.Log($"LocalHUD ShowPersistentMessage: {message}");

        if (messageRoutine != null)
        {
            StopCoroutine(messageRoutine);
            messageRoutine = null;
        }

        if (messageText != null)
            messageText.text = message;
        else
            Debug.LogWarning("LocalHUD: messageText is NULL");

        if (messageRoot != null)
            messageRoot.SetActive(true);
        else
            Debug.LogWarning("LocalHUD: messageRoot is NULL");
    }

    public void HideMessage()
    {
        Debug.Log("LocalHUD HideMessage");

        if (messageRoutine != null)
        {
            StopCoroutine(messageRoutine);
            messageRoutine = null;
        }

        if (messageRoot != null)
            messageRoot.SetActive(false);
    }

    private IEnumerator ShowMessageRoutine(string message, float seconds)
    {
        if (messageText != null)
            messageText.text = message;
        else
            Debug.LogWarning("LocalHUD: messageText is NULL");

        if (messageRoot != null)
            messageRoot.SetActive(true);
        else
            Debug.LogWarning("LocalHUD: messageRoot is NULL");

        yield return new WaitForSeconds(seconds);

        if (messageRoot != null)
            messageRoot.SetActive(false);

        messageRoutine = null;
    }
}