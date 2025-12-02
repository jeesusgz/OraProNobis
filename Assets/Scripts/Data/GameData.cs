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
    public int velocidadJugadorNivel = 0;

    [Header("Nazarenos")]
    public int cantidadNazarenos = 0;
}