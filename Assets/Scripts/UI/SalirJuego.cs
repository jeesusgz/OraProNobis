using UnityEngine;

public class SalirJuego : MonoBehaviour
{
    public void Salir()
    {
        // Sale de la aplicación
        Application.Quit();

        // Esto solo sirve en el editor para probar
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}