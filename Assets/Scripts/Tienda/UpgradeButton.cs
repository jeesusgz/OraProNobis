using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UpgradeButton : MonoBehaviour
{
    public int precio = 10;
    public Button button;

    public enum UpgradeType { Jugador, Paso, Nazarenos }
    public UpgradeType tipoUpgrade;

    public GameObject mensajePrefab; // Prefab de texto que se mostrará
    public Transform canvasTransform; // Canvas donde se mostrará el mensaje

    void Update()
    {
        if (CurrencyManager.Instance != null && button != null)
        {
            // Activar o desactivar según monedas
            button.interactable = CurrencyManager.Instance.gameData.monedas >= precio;
        }
    }

    public void ComprarMejora()
    {
        if (CurrencyManager.Instance.TrySpend(precio))
        {
            // Subir nivel según tipo
            switch (tipoUpgrade)
            {
                case UpgradeType.Jugador:
                    CurrencyManager.Instance.gameData.vidaJugadorNivel++;
                    break;
                case UpgradeType.Paso:
                    CurrencyManager.Instance.gameData.vidaPasoNivel++;
                    break;
                case UpgradeType.Nazarenos:
                    CurrencyManager.Instance.gameData.cantidadNazarenos++;
                    CurrencyManager.Instance.gameData.vidaNazarenoNivel++;

                    if (CurrencyManager.Instance.gameData.cantidadNazarenos > 4)
                        CurrencyManager.Instance.gameData.cantidadNazarenos = 4;

                    NazarenoHealthSystem[] nazarenos = FindObjectsOfType<NazarenoHealthSystem>();
                    foreach (var n in nazarenos)
                    {
                        n.SubirNivelVida(); // Añade +2 de vida a cada nazareno
                    }
                    break;
            }

            // Guardar
            SaveSystem.Save(CurrencyManager.Instance.gameData);

            // Mostrar mensaje
            if (mensajePrefab != null && canvasTransform != null)
            {
                GameObject mensaje = Instantiate(mensajePrefab, canvasTransform);
                mensaje.GetComponent<UnityEngine.UI.Text>().text = $"¡{tipoUpgrade} sube de nivel!";
                StartCoroutine(DesaparecerMensaje(mensaje, 2f));
            }
        }
    }

    private IEnumerator DesaparecerMensaje(GameObject mensaje, float tiempo)
    {
        yield return new WaitForSeconds(tiempo);
        Destroy(mensaje);
    }
}
