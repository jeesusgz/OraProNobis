using UnityEngine;

public class TiendaManager : MonoBehaviour
{
    public GameData gameData;
    public CurrencyManager currency;

    // ===== COSTES BASE =====
    [Header("Costes base")]
    public int costoVidaPasoBase = 50;
    public int costoEstaminaPasoBase = 60;

    public int costoVidaJugadorBase = 50;
    public int costoDañoJugadorBase = 70;
    public int costoVelocidadJugadorBase = 40;

    public int costoNazareno = 120;

    // ================================
    //  MEJORAS DEL PASO
    // ================================

    public void ComprarVidaPaso()
    {
        int coste = CalcularCoste(costoVidaPasoBase, gameData.vidaPasoNivel);

        if (currency.TrySpend(coste))
        {
            gameData.vidaPasoNivel++;
            SaveSystem.Save(gameData);
        }
    }

    public void ComprarEstaminaPaso()
    {
        int coste = CalcularCoste(costoEstaminaPasoBase, gameData.estaminaPasoNivel);

        if (currency.TrySpend(coste))
        {
            gameData.estaminaPasoNivel++;
            SaveSystem.Save(gameData);
        }
    }

    // ================================
    //  MEJORAS DEL JUGADOR
    // ================================

    public void ComprarVidaJugador()
    {
        int coste = CalcularCoste(costoVidaJugadorBase, gameData.vidaJugadorNivel);

        if (currency.TrySpend(coste))
        {
            gameData.vidaJugadorNivel++;
            SaveSystem.Save(gameData);
        }
    }

    public void ComprarDañoJugador()
    {
        int coste = CalcularCoste(costoDañoJugadorBase, gameData.dañoJugadorNivel);

        if (currency.TrySpend(coste))
        {
            gameData.dañoJugadorNivel++;
            SaveSystem.Save(gameData);
        }
    }

    public void ComprarVelocidadJugador()
    {
        int coste = CalcularCoste(costoVelocidadJugadorBase, gameData.velocidadJugadorNivel);

        if (currency.TrySpend(coste))
        {
            gameData.velocidadJugadorNivel++;
            SaveSystem.Save(gameData);
        }
    }

    // ================================
    //  NAZARENOS (UNIDADES)
    // ================================

    public void ComprarNazareno()
    {
        if (currency.TrySpend(costoNazareno))
        {
            gameData.cantidadNazarenos++;
            SaveSystem.Save(gameData);
        }
    }

    // ================================
    //  HERRAMIENTAS
    // ================================

    int CalcularCoste(int baseCost, int nivelActual)
    {
        // Coste progresivo: base × (1 + nivel × 0.5)
        return Mathf.RoundToInt(baseCost * (1f + nivelActual * 0.5f));
    }
}
