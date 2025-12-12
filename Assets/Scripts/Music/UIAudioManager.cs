using UnityEngine;

public class UIAudioManager : MonoBehaviour
{
    public static UIAudioManager Instance;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip hoverClip;
    public AudioClip clickClip;
    public AudioClip errorClip;
    public AudioClip levelUpClip;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            if (audioSource == null) audioSource = GetComponent<AudioSource>();
            Debug.Log("[UIAudioManager] Inicializado y marcado DontDestroyOnLoad");
        }
        else if (Instance != this)
        {
            Debug.Log("[UIAudioManager] Duplicado detectado. Destruyendo instancia extra.");
            Destroy(gameObject);
        }
    }

    public void PlayHover()
    {
        if (audioSource == null)
        {
            Debug.LogWarning("[UIAudioManager] PlayHover: AudioSource es null");
            return;
        }
        if (hoverClip == null)
        {
            Debug.LogWarning("[UIAudioManager] PlayHover: hoverClip es null");
            return;
        }
        audioSource.PlayOneShot(hoverClip);
        Debug.Log("[UIAudioManager] PlayHover reproducido");
    }

    public void PlayClick()
    {
        if (audioSource == null)
        {
            Debug.LogWarning("[UIAudioManager] PlayClick: AudioSource es null");
            return;
        }
        if (clickClip == null)
        {
            Debug.LogWarning("[UIAudioManager] PlayClick: clickClip es null");
            return;
        }
        audioSource.PlayOneShot(clickClip);
        Debug.Log("[UIAudioManager] PlayClick reproducido");
    }

    public void PlayError()
    {
        if (errorClip == null)
        {
            Debug.LogWarning("[UIAudioManager] ERROR: errorClip no asignado");
            return;
        }

        audioSource.PlayOneShot(errorClip);
        Debug.Log("[UIAudioManager] PlayError reproducido");
    }

    public void PlayLevelUp()
    {
        if (audioSource != null && levelUpClip != null)
        {
            audioSource.PlayOneShot(levelUpClip);
        }
    }
}