using UnityEngine;

public class SFXManager : MonoBehaviour
{
    public static SFXManager Instance { get; private set; }

    [Header("Audio Sources")]
    [SerializeField] private AudioSource oneShotSource;
    [SerializeField] private AudioSource glitchLoopSource;
    [SerializeField] private AudioSource footstepsLoopSource;

    [Header("Clips")]
    [SerializeField] private AudioClip beep;
    [SerializeField] private AudioClip clickbeep;
    [SerializeField] private AudioClip glitch;
    [SerializeField] private AudioClip footstepsLoop;
    [SerializeField] private AudioClip error;
    [SerializeField] private AudioClip success;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void PlayOneShot(AudioClip clip)
    {
        if (oneShotSource == null || clip == null)
            return;

        oneShotSource.PlayOneShot(clip);
    }

    public static void PlayBeep()
    {
        if (Instance != null)
            Instance.PlayOneShot(Instance.beep);
    }

    public static void PlayClickBeep()
    {
        if (Instance != null)
            Instance.PlayOneShot(Instance.clickbeep);
    }
    public static void PlayError()
    {
        if (Instance != null)
            Instance.PlayOneShot(Instance.error);
    }
    public static void PlaySuccess()
    {
        if (Instance != null)
            Instance.PlayOneShot(Instance.success);
    }

    public static void StartGlitchLoop()
    {
        if (Instance == null || Instance.glitchLoopSource == null || Instance.glitch == null)
            return;

        if (Instance.glitchLoopSource.isPlaying && Instance.glitchLoopSource.clip == Instance.glitch)
            return;

        Instance.glitchLoopSource.clip = Instance.glitch;
        Instance.glitchLoopSource.loop = true;
        Instance.glitchLoopSource.Play();
    }

    public static void StopGlitchLoop()
    {
        if (Instance == null || Instance.glitchLoopSource == null)
            return;

        Instance.glitchLoopSource.Stop();
        Instance.glitchLoopSource.clip = null;
    }

    public static void StartFootstepsLoop()
    {
        if (Instance == null || Instance.footstepsLoopSource == null || Instance.footstepsLoop == null)
            return;

        if (Instance.footstepsLoopSource.isPlaying && Instance.footstepsLoopSource.clip == Instance.footstepsLoop)
            return;

        Instance.footstepsLoopSource.clip = Instance.footstepsLoop;
        Instance.footstepsLoopSource.loop = true;
        Instance.footstepsLoopSource.Play();
    }

    public static void StopFootstepsLoop()
    {
        if (Instance == null || Instance.footstepsLoopSource == null)
            return;

        if (Instance.footstepsLoopSource.isPlaying)
            Instance.footstepsLoopSource.Stop();

        Instance.footstepsLoopSource.clip = null;
    }
}