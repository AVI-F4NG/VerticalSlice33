using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ComputerMonitorUI : MonoBehaviour
{
    [Header("UI Roots")]
    [SerializeField] private GameObject screenRoot;
    [SerializeField] private GameObject flowerHintPanel;
    [SerializeField] private GameObject virusPanel;
    [SerializeField] private Image wildFlashOverlay;

    [Header("Virus UI")]
    [SerializeField] private TMP_Text ejectButtonLabel;

    [Header("Monitor Sequence")]
    [SerializeField] private MonitorSequenceController monitorSequence;
    [SerializeField] private float wildFlashDuration = 0.6f;
    [SerializeField] private float wildFlashInterval = 0.06f;

    private bool ejectChanged;
    private bool continueRoutineRunning;

    public bool IsOpen { get; private set; }
    public bool IsInteractionLocked => GameSession.computerLockedAfterUSBUse;

    private void Start()
    {
        CloseImmediate();

        if (GameSession.monitorSequenceStarted && monitorSequence != null)
            monitorSequence.BeginLoop();
    }

    public void OpenScreen()
    {
        if (IsOpen || IsInteractionLocked)
            return;

        IsOpen = true;
        ejectChanged = false;

        if (ejectButtonLabel != null)
            ejectButtonLabel.text = "[EJECT DISK]";

        if (screenRoot != null)
            screenRoot.SetActive(true);

        if (flowerHintPanel != null)
            flowerHintPanel.SetActive(false);

        if (wildFlashOverlay != null)
            wildFlashOverlay.gameObject.SetActive(false);

        Debug.Log($"ComputerMonitorUI: usbFound={GameSession.usbFound}");

        if (virusPanel != null)
            virusPanel.SetActive(GameSession.usbFound);
    }

    public void OpenFlowerHint()
    {
        if (flowerHintPanel != null)
            flowerHintPanel.SetActive(true);
    }

    public void CloseFlowerHint()
    {
        if (flowerHintPanel != null)
            flowerHintPanel.SetActive(false);
    }

    public void OnContinuePressed()
    {
        if (!continueRoutineRunning)
            StartCoroutine(ContinueRoutine());
    }

    public void OnEjectPressed()
    {
        if (!ejectChanged)
        {
            ejectChanged = true;

            if (ejectButtonLabel != null)
                ejectButtonLabel.text = "[CONTINUE!]";

            return;
        }

        OnContinuePressed();
    }

    private IEnumerator ContinueRoutine()
    {
        continueRoutineRunning = true;

        GameSession.computerLockedAfterUSBUse = true;
        GameSession.monitorSequenceStarted = true;

        yield return StartCoroutine(WildFlashRoutine());

        CloseImmediate();

        if (monitorSequence != null)
            monitorSequence.BeginLoop();

        continueRoutineRunning = false;
    }

    private IEnumerator WildFlashRoutine()
    {
        if (wildFlashOverlay == null)
            yield break;

        wildFlashOverlay.gameObject.SetActive(true);

        float elapsed = 0f;

        while (elapsed < wildFlashDuration)
        {
            wildFlashOverlay.color = new Color(Random.value, Random.value, Random.value, 0.9f);
            yield return new WaitForSeconds(wildFlashInterval);
            elapsed += wildFlashInterval;
        }

        wildFlashOverlay.gameObject.SetActive(false);
    }

    public void CloseImmediate()
    {
        IsOpen = false;

        if (flowerHintPanel != null)
            flowerHintPanel.SetActive(false);

        if (virusPanel != null)
            virusPanel.SetActive(false);

        if (wildFlashOverlay != null)
            wildFlashOverlay.gameObject.SetActive(false);

        if (screenRoot != null)
            screenRoot.SetActive(false);

        UIKeyboardLock.Unlock();
    }
}