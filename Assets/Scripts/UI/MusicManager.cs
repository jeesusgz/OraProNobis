using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance;

    [Header("Música de Fondo")]
    public AudioSource audioSource;
    public AudioClip musicaNormal;
    public AudioClip musicaMuerte;
    public AudioClip musicaVictoria;

    private void Awake()
    {
        Instance = this;
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Si estamos en el menú principal, ponemos la música normal
        if (scene.name == "MenuPrincipal")
        {
            CambiarAMusicaNormal();
        }
    }

    public void CambiarAMusicaNormal()
    {
        if (audioSource == null || musicaNormal == null) return;

        audioSource.clip = musicaNormal;
        audioSource.loop = true;
        audioSource.Play();
    }

    public void CambiarAMusicaMuerte()
    {
        if (audioSource == null || musicaMuerte == null) return;

        audioSource.clip = musicaMuerte;
        audioSource.loop = true;
        audioSource.Play();
    }

    public void ReiniciarJuego()
    {
        MusicManager.Instance?.CambiarAMusicaNormal();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void CambiarAMusicaVictoria()
    {
        if (audioSource == null || musicaVictoria == null) return;

        audioSource.clip = musicaVictoria;
        audioSource.loop = true;
        audioSource.Play();
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}