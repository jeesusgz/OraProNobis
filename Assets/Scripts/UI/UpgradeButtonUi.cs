using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeButtonUI : MonoBehaviour
{
    public Button button;
    public Image coinImage;
    public TMP_Text priceText;
    public GameObject mensajePrefab;
    public Transform canvasTransform;

    public UpgradeButton buttonScript;

    private static GameObject mensajeActivo;

    void Update()
    {
        if (CurrencyManager.Instance == null || button == null || priceText == null || buttonScript == null)
            return;

        int precioActual = buttonScript.PrecioActual;
        int monedas = CurrencyManager.Instance.gameData.monedas;
        bool estaAlMaximo = false;

        // 🔥 Comprobación especial para NazarenosCantidad
        if (buttonScript.tipoUpgrade == UpgradeButton.UpgradeType.NazarenosCantidad)
        {
            PasoController paso = FindObjectOfType<PasoController>();
            if (paso != null)
            {
                int nazarenosActivos = 0;
                foreach (var n in paso.slotsDelante)
                    if (n != null) nazarenosActivos++;
                foreach (var n in paso.slotsDetras)
                    if (n != null) nazarenosActivos++;

                estaAlMaximo = nazarenosActivos >= (paso.maxSlotsDelante + paso.maxSlotsDetras);
            }
        }
        else
        {
            // Para otros upgrades usamos la lógica normal
            estaAlMaximo = buttonScript.EstaMaximo();
        }

        if (estaAlMaximo)
        {
            button.interactable = false;

            if (coinImage != null)
                coinImage.gameObject.SetActive(false);

            priceText.text = "MAX";
            priceText.color = Color.yellow;
            return;
        }

        if (coinImage != null)
            coinImage.gameObject.SetActive(true);

        priceText.text = precioActual.ToString();
        priceText.color = monedas >= precioActual ? Color.green : Color.red;

        button.interactable = monedas >= precioActual;
    }

    // 🔥 SE EJECUTA AL CAMBIAR DE MENÚ O DESACTIVAR EL GAMEOBJECT
    void OnDisable()
    {
        if (mensajeActivo != null)
        {
            Destroy(mensajeActivo);
            mensajeActivo = null;
        }
    }

    public void MostrarMensaje(string texto)
    {
        if (mensajePrefab != null && canvasTransform != null)
        {
            if (mensajeActivo != null)
                Destroy(mensajeActivo);

            mensajeActivo = Instantiate(mensajePrefab, canvasTransform);
            TMP_Text tmp = mensajeActivo.GetComponent<TMP_Text>();
            if (tmp != null) tmp.text = texto;

            StartCoroutine(DesaparecerMensaje(mensajeActivo, 2f));
        }
    }

    private IEnumerator DesaparecerMensaje(GameObject mensaje, float tiempo)
    {
        yield return new WaitForSeconds(tiempo);

        if (mensaje == mensajeActivo)
            mensajeActivo = null;

        Destroy(mensaje);
    }

    public void MostrarMensajeMejora(UpgradeButton.UpgradeType tipoUpgrade)
    {
        string mensaje = tipoUpgrade switch
        {
            UpgradeButton.UpgradeType.JugadorVida => "+1 Vida del Jugador",
            UpgradeButton.UpgradeType.JugadorFuerza => "+1 Daño del Jugador",
            UpgradeButton.UpgradeType.PasoVida => "+10 Vida del Paso",
            UpgradeButton.UpgradeType.PasoEstamina => "+5 Estamina del Paso",
            UpgradeButton.UpgradeType.PasoVelocidad => "+0.5 Velocidad del Paso",
            UpgradeButton.UpgradeType.NazarenosCantidad => "+1 Nazareno (máx 4)",
            UpgradeButton.UpgradeType.NazarenosVida => "+1 Vida de los Nazarenos",
            _ => "Mejora aplicada"
        };

        MostrarMensaje(mensaje);
    }

    public void MostrarMensajeSinDinero()
    {
        MostrarMensaje("No tienes suficientes monedas");
    }

    public void MostrarMensajeMaximo()
    {
        MostrarMensaje("Ya está al nivel máximo");
    }
}