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
            GameObject mensaje = Instantiate(mensajePrefab, canvasTransform);
            TMP_Text tmp = mensaje.GetComponent<TMP_Text>();
            if (tmp != null) tmp.text = texto;
            StartCoroutine(DesaparecerMensaje(mensaje, 2f));
        }
    }

    private IEnumerator DesaparecerMensaje(GameObject mensaje, float tiempo)
    {
        yield return new WaitForSeconds(tiempo);
        Destroy(mensaje);
    }
}