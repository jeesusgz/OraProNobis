using UnityEngine;

public class UpgradeButton : MonoBehaviour
{
    public int precioBase = 10;
    public int incrementoPrecio = 5;

    public enum UpgradeType
    {
        JugadorVida,
        JugadorFuerza,
        PasoVida,
        PasoEstamina,
        PasoVelocidad,
        NazarenosCantidad,
        NazarenosVida
    }

    public UpgradeType tipoUpgrade;

    [HideInInspector]
    public int nivelBoton = 0;

    public UpgradeButtonUI buttonUI; // <<-- Asignar en el inspector

    void Start()
    {
        if (CurrencyManager.Instance == null) return;

        switch (tipoUpgrade)
        {
            case UpgradeType.JugadorVida: nivelBoton = CurrencyManager.Instance.gameData.jugadorVidaBotonNivel; break;
            case UpgradeType.JugadorFuerza: nivelBoton = CurrencyManager.Instance.gameData.jugadorFuerzaBotonNivel; break;
            case UpgradeType.PasoVida: nivelBoton = CurrencyManager.Instance.gameData.pasoVidaBotonNivel; break;
            case UpgradeType.PasoEstamina: nivelBoton = CurrencyManager.Instance.gameData.pasoEstaminaBotonNivel; break;
            case UpgradeType.PasoVelocidad: nivelBoton = CurrencyManager.Instance.gameData.pasoVelocidadBotonNivel; break;
            case UpgradeType.NazarenosCantidad: nivelBoton = CurrencyManager.Instance.gameData.nazarenosCantidadBotonNivel; break;
            case UpgradeType.NazarenosVida: nivelBoton = CurrencyManager.Instance.gameData.nazarenosVidaBotonNivel; break;
        }
    }

    public int PrecioActual => precioBase + nivelBoton * incrementoPrecio;

    public void ComprarMejora()
    {
        if (CurrencyManager.Instance == null) return;

        int precio = PrecioActual;
        if (!CurrencyManager.Instance.TrySpend(precio)) return;

        nivelBoton++;

        // Aplicamos el efecto y guardamos el nivel del botón
        switch (tipoUpgrade)
        {
            case UpgradeType.JugadorVida:
                CurrencyManager.Instance.gameData.vidaJugadorNivel++;
                CurrencyManager.Instance.gameData.jugadorVidaBotonNivel = nivelBoton;
                break;

            case UpgradeType.JugadorFuerza:
                CurrencyManager.Instance.gameData.dañoJugadorNivel++;
                CurrencyManager.Instance.gameData.jugadorFuerzaBotonNivel = nivelBoton;
                break;

            case UpgradeType.PasoVida:
                CurrencyManager.Instance.gameData.vidaPasoNivel++;
                CurrencyManager.Instance.gameData.pasoVidaBotonNivel = nivelBoton;
                break;

            case UpgradeType.PasoEstamina:
                CurrencyManager.Instance.gameData.estaminaPasoNivel++;
                CurrencyManager.Instance.gameData.pasoEstaminaBotonNivel = nivelBoton;
                break;

            case UpgradeType.PasoVelocidad:
                CurrencyManager.Instance.gameData.velocidadPasoNivel++;
                CurrencyManager.Instance.gameData.pasoVelocidadBotonNivel = nivelBoton;
                break;

            case UpgradeType.NazarenosCantidad:
                CurrencyManager.Instance.gameData.cantidadNazarenos++;
                if (CurrencyManager.Instance.gameData.cantidadNazarenos > 4)
                    CurrencyManager.Instance.gameData.cantidadNazarenos = 4;
                CurrencyManager.Instance.gameData.nazarenosCantidadBotonNivel = nivelBoton;
                break;

            case UpgradeType.NazarenosVida:
                CurrencyManager.Instance.gameData.vidaNazarenoNivel++;
                CurrencyManager.Instance.gameData.nazarenosVidaBotonNivel = nivelBoton;
                NazarenoHealthSystem[] nazarenos = FindObjectsOfType<NazarenoHealthSystem>();
                foreach (var n in nazarenos)
                    n.SubirNivelVida();
                break;
        }

        SaveSystem.Save(CurrencyManager.Instance.gameData);

        // <<-- Mostramos el mensaje emergente después de la compra
        if (buttonUI != null)
        {
            buttonUI.MostrarMensajeMejora(tipoUpgrade);
        }
    }
}
