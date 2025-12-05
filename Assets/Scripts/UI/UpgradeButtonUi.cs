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

    public UpgradeButton buttonScript; // Referencia al UpgradeButton

    private static GameObject mensajeActivo; // <<-- static para un mensaje global

    void Update()
    {
        if (CurrencyManager.Instance == null || button == null || priceText == null || buttonScript == null)
            return;

        int precioActual = buttonScript.PrecioActual;
        bool puedeComprar = CurrencyManager.Instance.gameData.monedas >= precioActual;

        button.interactable = puedeComprar;
        priceText.color = puedeComprar ? Color.green : Color.red;
        priceText.text = precioActual.ToString();
    }

    public void MostrarMensaje(string texto)
    {
        if (mensajePrefab != null && canvasTransform != null)
        {
            // Destruir mensaje antiguo si existe
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
}