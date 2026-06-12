using System.Collections;
using TMPro;
using UnityEngine;

public class DoorAPasswordUI : MonoBehaviour
{
    [SerializeField] private GameObject panelRoot;
    [SerializeField] private TMP_Text entryText;
    [SerializeField] private string correctCode = "1305";
    [SerializeField] private DoorAController doorController;
    [SerializeField] private KeyCode closeKey = KeyCode.Escape;
    [SerializeField] private float successDelay = 0.6f;

    private string currentEntry = "";
    private bool evaluating;
    private bool success;
    private bool isOpen;

    public bool IsOpen => isOpen;

    private void Start()
    {
        CloseImmediate();
    }

    private void Update()
    {
        if (!isOpen)
            return;

        if (Input.GetKeyDown(closeKey))
            ClosePanel();
    }

    public void OpenPanel()
    {
        if (GameSession.doorAUnlocked)
            return;

        if (panelRoot != null)
            panelRoot.SetActive(true);

        currentEntry = "";
        evaluating = false;
        success = false;
        isOpen = true;

        if (entryText != null)
            entryText.text = "";

        UIKeyboardLock.Lock();
    }

    public void ClosePanel()
    {
        if (panelRoot != null)
            panelRoot.SetActive(false);

        isOpen = false;
        UIKeyboardLock.Unlock();

        if (success)
        {
            GameSession.doorAUnlocked = true;

            if (doorController != null)
                doorController.UnlockDoorSilently();
        }
    }

    public void PressDigit(string digit)
    {
        if (!isOpen || evaluating || success) return;
        if (string.IsNullOrEmpty(digit) || digit.Length != 1) return;
        if (currentEntry.Length >= 4) return;

        SFXManager.PlayBeep();

        currentEntry += digit;

        if (entryText != null)
            entryText.text = currentEntry;

        if (currentEntry.Length == 4)
            StartCoroutine(EvaluateEntry());
    }

    public void DeleteDigit()
    {
        if (!isOpen || evaluating || success) return;
        if (currentEntry.Length >= 4 || currentEntry.Length < 1) return;

        currentEntry = currentEntry.Substring(0, currentEntry.Length - 1); 

        if (entryText != null)
            entryText.text = currentEntry;
    }

    private IEnumerator EvaluateEntry()
    {
        evaluating = true;

        if (currentEntry == correctCode)
        {
            success = true;

            if (entryText != null){
                SFXManager.PlaySuccess();
                entryText.text = "SUCCESS";
            }

            yield return new WaitForSeconds(successDelay);

            ClosePanel();
        }
        else
        {
            if (entryText != null){
                SFXManager.PlayError();
                entryText.text = "<color=\"red\">ERROR</color>";
            }

            yield return new WaitForSeconds(2f);

            currentEntry = "";

            if (entryText != null)
                entryText.text = "";
        }

        evaluating = false;
    }

    public void CloseImmediate()
    {
        currentEntry = "";
        evaluating = false;
        success = false;
        isOpen = false;

        if (entryText != null)
            entryText.text = "";

        if (panelRoot != null)
            panelRoot.SetActive(false);

        UIKeyboardLock.Unlock();
    }
}