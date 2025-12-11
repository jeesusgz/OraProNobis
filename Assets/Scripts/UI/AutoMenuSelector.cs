using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AutoMenuSelector : MonoBehaviour
{
    void OnEnable()
    {
        SeleccionarPrimerBotonActivo();
    }

    void SeleccionarPrimerBotonActivo()
    {
        // Limpiar cualquier selección previa
        EventSystem.current.SetSelectedGameObject(null);

        // Buscar todos los botones activos en la escena
        Button[] botones = GameObject.FindObjectsOfType<Button>(true); // true incluye inactivos
        foreach (Button btn in botones)
        {
            if (!btn.gameObject.activeInHierarchy) continue; // Ignorar inactivos
            if (!btn.interactable) continue; // Ignorar no interactuables

            // Seleccionar el primer botón válido
            EventSystem.current.SetSelectedGameObject(btn.gameObject);
            return;
        }
    }
}