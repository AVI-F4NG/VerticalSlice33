using System.Collections;
using TMPro;
using UnityEngine;

public class DoorAPasswordUI : MonoBehaviour
{
    [SerializeField] private GameObject panelRoot;
    [SerializeField] private TMP_Text entryText;
    [SerializeField] private string correctCode = "1305";
    [SerializeField] private DoorAController doorController;

    private string currentEntry = "";
    private bool evaluating;
    private bool success;

    private void Start()
    {
        CloseImmediate();
    }

    public void OpenPanel()
    {
        if (panelRoot != null)
            panelRoot.SetActive(true);

        currentEntry = "";
        evaluating = false;
        success = false;

        if (entryText != null)
            entryText.text = "";
    }

    public void ClosePanel()
    {
        if (panelRoot != null)
            panelRoot.SetActive(false);

        if (success && doorController != null)
            doorController.UnlockDoor();
    }

    public void PressDigit(string digit)
    {
        if (evaluating || success) return;
        if (string.IsNullOrEmpty(digit) || digit.Length != 1) return;
        if (currentEntry.Length >= 4) return;

        currentEntry += digit;

        if (entryText != null)
            entryText.text = currentEntry;

        if (currentEntry.Length == 4)
            StartCoroutine(EvaluateEntry());
    }

    private IEnumerator EvaluateEntry()
    {
        evaluating = true;

        if (currentEntry == correctCode)
        {
            success = true;

            if (entryText != null)
                entryText.text = "SUCCESS";
        }
        else
        {
            if (entryText != null)
                entryText.text = "FAILURE";

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

        if (entryText != null)
            entryText.text = "";

        if (panelRoot != null)
            panelRoot.SetActive(false);
    }
}