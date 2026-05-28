using UnityEngine;

public class PictureReveal : MonoBehaviour
{
    [SerializeField] private Transform pictureTransform;
    [SerializeField] private Vector3 revealedLocalPosition;
    [SerializeField] private GameObject usbObject;
    [SerializeField] private string pictureMessage;

    private Vector3 hiddenLocalPosition;
    public bool revealed;

    private void Awake()
    {
        if (pictureTransform == null)
            pictureTransform = transform;

        hiddenLocalPosition = pictureTransform.localPosition;
    }

    private void Start()
    {
        if (GameSession.usbFound)
        {
            pictureTransform.localPosition = revealedLocalPosition;

            if (usbObject != null)
                usbObject.SetActive(false);

            revealed = true;
            return;
        }

        pictureTransform.localPosition = hiddenLocalPosition;

        if (usbObject != null)
            usbObject.SetActive(false);

        revealed = false;
    }

    public void Interact()
    {
        if (revealed)
            return;

        pictureTransform.localPosition = revealedLocalPosition;

        if (usbObject != null && !GameSession.usbFound)
            usbObject.SetActive(true);

        revealed = true;
    }
}