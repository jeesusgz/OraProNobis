using UnityEngine;

public class UpgradeButton : MonoBehaviour
{
    public PasoController pasoController;
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

    public UpgradeButtonUI buttonUI; // Asignar en el inspector

    public int maxJugadorVida = 42;
    public int maxJugadorFuerza = 10;
    public int maxPasoVida = 100;
    public int maxPasoEstamina = 100;
    public int maxPasoVelocidad = 3;
    //public int maxNazarenosCantidad = 4; // límite natural
    public int maxNazarenosVida = 10;

    void Start()
    {
        if (CurrencyManager.Instance == null) return;

        // Inicializar nivel del botón según el tipo de mejora
        switch (tipoUpgrade)
        {
            case UpgradeType.JugadorVida: nivelBoton = CurrencyManager.Instance.gameData.jugadorVidaBotonNivel; break;
            case UpgradeType.JugadorFuerza: nivelBoton = CurrencyManager.Instance.gameData.jugadorFuerzaBotonNivel; break;
            case UpgradeType.PasoVida: nivelBoton = CurrencyManager.Instance.gameData.pasoVidaBotonNivel; break;
            case UpgradeType.PasoEstamina: nivelBoton = CurrencyManager.Instance.gameData.pasoEstaminaBotonNivel; break;
            case UpgradeType.PasoVelocidad: nivelBoton = CurrencyManager.Instance.gameData.pasoVelocidadBotonNivel; break;
            case UpgradeType.NazarenosCantidad: /* No usamos nivelBoton */ break;
            case UpgradeType.NazarenosVida: nivelBoton = CurrencyManager.Instance.gameData.nazarenosVidaBotonNivel; break;
        }
    }

    public bool EstaMaximo()
    {
        if (tipoUpgrade == UpgradeType.NazarenosCantidad)
        {
            // ✅ SIEMPRE usa GameData (funciona entre escenas)
            return CurrencyManager.Instance.gameData.cantidadNazarenos >= 4;
        }

        // Otros upgrades iguales...
        return tipoUpgrade switch
        {
            UpgradeType.JugadorVida => nivelBoton >= maxJugadorVida,
            UpgradeType.JugadorFuerza => nivelBoton >= maxJugadorFuerza,
            UpgradeType.PasoVida => nivelBoton >= maxPasoVida,
            UpgradeType.PasoEstamina => nivelBoton >= maxPasoEstamina,
            UpgradeType.PasoVelocidad => nivelBoton >= maxPasoVelocidad,
            UpgradeType.NazarenosVida => nivelBoton >= maxNazarenosVida,
            _ => false
        };
    }

    public int PrecioActual
    {
        get
        {
            if (tipoUpgrade == UpgradeType.NazarenosCantidad)
            {
                // 🔥 PRECIO PROGRESIVO basado en nazarenos YA comprados
                int nazarenosActuales = CurrencyManager.Instance.gameData.cantidadNazarenos;
                return precioBase + (nazarenosActuales * incrementoPrecio);
            }

            return precioBase + nivelBoton * incrementoPrecio;
        }
    }

    public void ComprarMejora()
    {
        Debug.Log($"🔥 1. INICIO {tipoUpgrade}");

        if (CurrencyManager.Instance == null)
        {
            Debug.LogError("❌ 2. CurrencyManager NULL");
            return;
        }
        Debug.Log($"✅ 2. CurrencyManager OK | Monedas: {CurrencyManager.Instance.gameData.monedas}");

        // 🔥 NAZARENOSCANTIDAD: SIN PASOCONTROLLER (funciona entre escenas)
        if (tipoUpgrade == UpgradeType.NazarenosCantidad)
        {
            Debug.Log("🔥 3. NazarenosCantidad (GameData ONLY)");

            if (CurrencyManager.Instance.gameData.cantidadNazarenos >= 4)
            {
                Debug.Log("❌ 4. Máximo 4 nazarenos");
                buttonUI?.MostrarMensajeMaximo();
                return;
            }

            int precio = PrecioActual;
            Debug.Log($"💰 5. Precio: {precio} | Actual: {CurrencyManager.Instance.gameData.cantidadNazarenos}/4");

            if (!CurrencyManager.Instance.TrySpend(precio))
            {
                Debug.Log("❌ 6. Sin dinero");
                buttonUI?.MostrarMensajeSinDinero();
                return;
            }

            // ✅ INCREMENTAR GAME DATA
            CurrencyManager.Instance.gameData.cantidadNazarenos++;
            SaveSystem.Save(CurrencyManager.Instance.gameData);

            buttonUI?.MostrarMensajeMejora(tipoUpgrade);
            Debug.Log($"✅ 7. Nazareno #{CurrencyManager.Instance.gameData.cantidadNazarenos}/4 COMPRADO!");
            return;
        }

        // 🔹 LÓGICA NORMAL (arreglado el switch)
        Debug.Log("🔥 3. Upgrade normal");

        if (EstaMaximo())
        {
            buttonUI?.MostrarMensajeMaximo();
            return;
        }

        int precioNormal = PrecioActual;
        if (!CurrencyManager.Instance.TrySpend(precioNormal))
        {
            buttonUI?.MostrarMensajeSinDinero();
            return;
        }

        nivelBoton++;
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
            case UpgradeType.NazarenosVida:
                CurrencyManager.Instance.gameData.vidaNazarenoNivel++;
                CurrencyManager.Instance.gameData.nazarenosVidaBotonNivel = nivelBoton;
                NazarenoHealthSystem[] nazarenos = FindObjectsOfType<NazarenoHealthSystem>();
                foreach (var n in nazarenos)
                    n.SubirNivelVida();
                break;
        }

        SaveSystem.Save(CurrencyManager.Instance.gameData);
        buttonUI?.MostrarMensajeMejora(tipoUpgrade);
        Debug.Log("✅ Upgrade aplicado!");
    }
}