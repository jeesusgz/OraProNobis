using UnityEngine;

public class ZonaInteraccion : MonoBehaviour
{
    public PasoController paso;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            paso.jugadorCerca = true;
            paso.jugador = other.transform;
            Debug.Log("Jugador cerca del paso.");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            paso.jugadorCerca = false;
            paso.jugador = null;
            Debug.Log("Jugador se aleja del paso.");
        }
    }
}