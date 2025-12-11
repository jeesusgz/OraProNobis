using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class MainMenu : MonoBehaviour
{
    public GameObject botonContinuar;
    public GameObject botonNuevaPartida;

    void OnEnable()
    {
        bool existe = SaveSystem.SaveExists();

        botonContinuar.SetActive(existe);
        botonNuevaPartida.SetActive(true);

        //Seleccionar correctamente el botón inicial
        EventSystem.current.SetSelectedGameObject(null);

        if (existe)
            EventSystem.current.SetSelectedGameObject(botonContinuar);
        else
            EventSystem.current.SetSelectedGameObject(botonNuevaPartida);
    }

    public void NuevaPartida()
    {
        SaveSystem.DeleteSave();

        CurrencyManager.Instance.gameData.ResetData();
        SaveSystem.Save(CurrencyManager.Instance.gameData);

        SceneManager.LoadScene("Dia");
    }

    public void Continuar()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Dia");
    }
}
