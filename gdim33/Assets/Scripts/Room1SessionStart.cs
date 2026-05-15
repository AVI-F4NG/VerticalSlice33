using UnityEngine;

public sealed class Room1SessionStart : MonoBehaviour
{
    [SerializeField] private bool resetOnlyOncePerPlaySession = true;

    private void Awake()
    {
        if (!resetOnlyOncePerPlaySession)
        {
            GameSession.ResetAll();
            return;
        }

        if (!GameSession.hasInitialized)
        {
            GameSession.ResetAll();
        }
    }
}