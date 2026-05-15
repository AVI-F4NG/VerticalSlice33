using System.Collections;
using UnityEngine;

public class MonitorSequenceController : MonoBehaviour
{
    [SerializeField] private GameObject[] flashObjects;
    [SerializeField] private float flashDuration = 0.5f;
    [SerializeField] private float waitTime = 2f;

    private Coroutine loopRoutine;

    private void Awake()
    {
        if (flashObjects == null) return;

        foreach (GameObject flash in flashObjects)
        {
            if (flash != null)
                flash.SetActive(false);
        }
    }

    public void BeginLoop()
    {
        if (loopRoutine == null)
            loopRoutine = StartCoroutine(LoopRoutine());
    }

    private IEnumerator LoopRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(waitTime);
            yield return StartCoroutine(PlayOnePass());

            yield return new WaitForSeconds(waitTime);
            yield return StartCoroutine(PlayOnePass());
        }
    }

    private IEnumerator PlayOnePass()
    {
        for (int i = 0; i < flashObjects.Length; i++)
        {
            if (flashObjects[i] != null)
                flashObjects[i].SetActive(true);

            yield return new WaitForSeconds(flashDuration);

            if (flashObjects[i] != null)
                flashObjects[i].SetActive(false);
        }
    }
}