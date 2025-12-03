using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ShopMenu : MonoBehaviour
{
    [Header("Paneles")]
    public GameObject panelPrincipal;
    public GameObject panelJugador;
    public GameObject panelPaso;
    public GameObject panelNazarenos;

    [Header("Botones Principales")]
    public Button botonJugador;
    public Button botonPaso;
    public Button botonNazarenos;
    public Button botonSalir; // Nuevo: salir al menú principal
    public Button botonJugar; // Nuevo: cargar la escena nocturna

    [Header("Botones Volver de Subpaneles")]
    public Button botonVolverJugador;
    public Button botonVolverPaso;
    public Button botonVolverNazarenos;

    void Start()
    {
        // Inicial
        panelPrincipal.SetActive(true);
        panelJugador.SetActive(false);
        panelPaso.SetActive(false);
        panelNazarenos.SetActive(false);

        // Botones principales
        botonJugador.onClick.AddListener(() => ShowPanel(panelJugador));
        botonPaso.onClick.AddListener(() => ShowPanel(panelPaso));
        botonNazarenos.onClick.AddListener(() => ShowPanel(panelNazarenos));
        botonSalir.onClick.AddListener(SalirAlMenuPrincipal);
        botonJugar.onClick.AddListener(CargarEscenaNoche); // Asociamos el botón Jugar

        // Botones de volver
        botonVolverJugador.onClick.AddListener(ShowPrincipal);
        botonVolverPaso.onClick.AddListener(ShowPrincipal);
        botonVolverNazarenos.onClick.AddListener(ShowPrincipal);
    }

    void ShowPanel(GameObject panel)
    {
        panelPrincipal.SetActive(false);
        panelJugador.SetActive(false);
        panelPaso.SetActive(false);
        panelNazarenos.SetActive(false);

        panel.SetActive(true);
    }

    void ShowPrincipal()
    {
        panelJugador.SetActive(false);
        panelPaso.SetActive(false);
        panelNazarenos.SetActive(false);
        panelPrincipal.SetActive(true);
    }

    void SalirAlMenuPrincipal()
    {
        SceneManager.LoadScene("MenuPrincipal"); // Nombre de tu escena principal
    }

    void CargarEscenaNoche()
    {
        // Guardamos el GameData antes de cargar la noche
        if (CurrencyManager.Instance != null)
        {
            SaveSystem.Save(CurrencyManager.Instance.gameData);
        }

        // Cambiamos a la escena nocturna (pon el nombre real de tu escena)
        SceneManager.LoadScene("Noche");
    }
}
