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

    [Header("World Screen Visual Change")]
    [SerializeField] private SpriteRenderer screenSpriteRenderer;
    [SerializeField] private Sprite changedScreenSprite;
    [SerializeField] private Material changedScreenMaterial;

    private bool ejectChanged;
    private bool continueRoutineRunning;

    private Sprite originalScreenSprite;
    private Material originalScreenMaterial;
    private Vector3 originalLocalScale;
    private bool cachedOriginalVisualState;

    public bool IsOpen { get; private set; }
    public bool IsInteractionLocked => GameSession.computerLockedAfterUSBUse;

    private void Awake()
    {
        CacheOriginalVisualState();
    }

    private void Start()
    {
        CloseImmediate();

        if (GameSession.monitorSequenceStarted)
        {
            ApplyPostVirusScreenVisual();

            if (monitorSequence != null)
                monitorSequence.BeginLoop();
        }
    }

    private void CacheOriginalVisualState()
    {
        if (cachedOriginalVisualState || screenSpriteRenderer == null)
            return;

        originalScreenSprite = screenSpriteRenderer.sprite;
        originalScreenMaterial = screenSpriteRenderer.sharedMaterial;
        originalLocalScale = screenSpriteRenderer.transform.localScale;
        cachedOriginalVisualState = true;
    }

    public bool OpenScreen()
    {
        if (IsOpen || IsInteractionLocked || screenRoot == null)
            return false;

        IsOpen = true;
        ejectChanged = false;

        if (ejectButtonLabel != null)
            ejectButtonLabel.text = "[EJECT DISK]";

        screenRoot.SetActive(true);

        if (flowerHintPanel != null)
            flowerHintPanel.SetActive(false);

        if (wildFlashOverlay != null)
            wildFlashOverlay.gameObject.SetActive(false);

        if (virusPanel != null)
            virusPanel.SetActive(GameSession.usbFound);

        return true;
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
        ApplyPostVirusScreenVisual();

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

    private void ApplyPostVirusScreenVisual()
    {
        if (screenSpriteRenderer == null || changedScreenSprite == null)
            return;

        CacheOriginalVisualState();

        // Swap material first if desired
        if (changedScreenMaterial != null)
            screenSpriteRenderer.material = changedScreenMaterial;
        else if (originalScreenMaterial != null)
            screenSpriteRenderer.material = originalScreenMaterial;

        // Compute exact scale ratio from sprite bounds
        Vector3 targetScale = originalLocalScale;

        Bounds originalBounds = originalScreenSprite.bounds;
        Bounds changedBounds = changedScreenSprite.bounds;

        if (changedBounds.size.x != 0f)
            targetScale.x = originalLocalScale.x * (originalBounds.size.x / changedBounds.size.x);

        if (changedBounds.size.y != 0f)
            targetScale.y = originalLocalScale.y * (originalBounds.size.y / changedBounds.size.y);

        // Keep original Z scale
        targetScale.z = originalLocalScale.z;

        screenSpriteRenderer.sprite = changedScreenSprite;
        screenSpriteRenderer.transform.localScale = targetScale;
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