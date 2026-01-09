using UnityEngine;

[CreateAssetMenu(fileName = "GameData", menuName = "Game/GameData")]
public class GameData : ScriptableObject
{
    [Header("Economía")]
    public int monedas = 0;

    [Header("Paso")]
    public int vidaPasoNivel = 0;
    public int estaminaPasoNivel = 0;
    public int velocidadPasoNivel = 0;

    [Header("Jugador")]
    public int vidaJugadorNivel = 0;
    public int dañoJugadorNivel = 0;
    public bool dobleSaltoComprado = false;

    [Header("Nazarenos")]
    public int cantidadNazarenos = 0;
    public int vidaNazarenoNivel = 0;

    [Header("Niveles de botones")]
    public int jugadorVidaBotonNivel = 0;
    public int jugadorFuerzaBotonNivel = 0;
    public int pasoVidaBotonNivel = 0;
    public int pasoEstaminaBotonNivel = 0;
    public int pasoVelocidadBotonNivel = 0;
    public int nazarenosCantidadBotonNivel = 0;
    public int nazarenosVidaBotonNivel = 0;
    public int jugadorVelocidadBotonNivel = 0;

    public void ResetData()
    {
        monedas = 0;
        vidaPasoNivel = 0;
        estaminaPasoNivel = 0;
        velocidadPasoNivel = 0;
        vidaJugadorNivel = 0;
        dañoJugadorNivel = 0;
        cantidadNazarenos = 0;
        vidaNazarenoNivel = 0;
        jugadorVidaBotonNivel = 0;
        jugadorFuerzaBotonNivel = 0;
        pasoVidaBotonNivel = 0;
        pasoEstaminaBotonNivel = 0;
        pasoVelocidadBotonNivel = 0;
        nazarenosCantidadBotonNivel = 0;
        nazarenosVidaBotonNivel = 0;
        jugadorVelocidadBotonNivel = 0;
    }
}