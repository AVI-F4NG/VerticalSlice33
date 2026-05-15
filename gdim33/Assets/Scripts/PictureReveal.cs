using UnityEngine;

public class PictureReveal : MonoBehaviour
{
    [SerializeField] private Transform pictureTransform;
    [SerializeField] private Vector3 revealedLocalPosition;
    [SerializeField] private GameObject usbObject;

    private bool revealed;

    private void Awake()
    {
        if (usbObject != null)
            usbObject.SetActive(false);
    }

    public void Interact()
    {
        if (revealed) return;

        if (pictureTransform != null)
            pictureTransform.localPosition = revealedLocalPosition;

        if (usbObject != null)
            usbObject.SetActive(true);

        revealed = true;
    }
}