public static class GameSession
{
    public static bool hasInitialized;

    public static bool usbFound;
    public static bool doorAUnlocked;
    public static bool hasFinalClearance;
    public static bool monitorSequenceStarted;
    public static bool computerLockedAfterUSBUse;

    public static void ResetAll()
    {
        usbFound = false;
        doorAUnlocked = false;
        hasFinalClearance = false;
        monitorSequenceStarted = false;
        computerLockedAfterUSBUse = false;
        hasInitialized = true;
    }
}