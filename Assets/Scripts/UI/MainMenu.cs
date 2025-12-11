using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject botonContinuar;
    public GameObject botonNuevaPartida;

    void OnEnable()
    {
        bool existe = SaveSystem.SaveExists();

        botonContinuar.SetActive(existe);
        botonNuevaPartida.SetActive(true);
    }

    public void NuevaPartida()
    {
        SaveSystem.DeleteSave();  // borra el archivo guardado

        CurrencyManager.Instance.gameData.ResetData(); // 🔥 resetea valores actuales en memoria

        SaveSystem.Save(CurrencyManager.Instance.gameData); // guarda vacío para evitar errores

        UnityEngine.SceneManagement.SceneManager.LoadScene("Dia"); // o tu escena de nivel
    }

    public void Continuar()
    {
        // Resetear TimeScale
        Time.timeScale = 1f;

        SceneManager.LoadScene("Dia");
    }
}
