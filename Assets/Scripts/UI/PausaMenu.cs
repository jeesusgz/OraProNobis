using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class PausaMenu : MonoBehaviour
{
    public GameObject menuPausa;
    public InputActionReference pauseAction; // Acción del nuevo Input System

    private bool juegoPausado = false;

    private void OnEnable()
    {
        pauseAction.action.Enable();
        pauseAction.action.performed += OnPause;
    }

    private void OnDisable()
    {
        pauseAction.action.performed -= OnPause;
        pauseAction.action.Disable();
    }

    private void OnPause(InputAction.CallbackContext context)
    {
        if (juegoPausado)
            Reanudar();
        else
            Pausar();
    }

    public void Pausar()
    {
        menuPausa.SetActive(true);
        Time.timeScale = 0f;
        juegoPausado = true;

        // Pausar la música de fondo
        if (MusicManager.Instance != null && MusicManager.Instance.audioSource != null)
            MusicManager.Instance.audioSource.Pause();
    }

    public void Reanudar()
    {
        menuPausa.SetActive(false);
        Time.timeScale = 1f;
        juegoPausado = false;

        // Reanudar la música de fondo
        if (MusicManager.Instance != null && MusicManager.Instance.audioSource != null)
            MusicManager.Instance.audioSource.UnPause();
    }

    public void ReiniciarNivel()
    {
        Time.timeScale = 1f;
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }

    public void SalirAlMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MenuPrincipal");
    }
}