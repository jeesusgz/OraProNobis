using UnityEngine;

public class NightPhaseManager : MonoBehaviour
{
    public PasoController paso;

    private void Start()
    {
        // Aquí puedes iniciar la noche automáticamente, por ejemplo
        StartNightPhase();
    }

    public void StartNightPhase()
    {
        if (paso == null) return;

        // Recorremos los slots disponibles
        int slotsDisponibles = 4; // 2 delante + 2 detrás
        int nazarenosComprados = paso.gameData.cantidadNazarenos;

        // Intentamos llenar los slots hasta el máximo permitido
        for (int i = 0; i < nazarenosComprados && i < slotsDisponibles; i++)
        {
            // Primero 2 delante
            bool delante = i < 2;
            paso.ComprarNazareno(delante);
        }
    }
}
