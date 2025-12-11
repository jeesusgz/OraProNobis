using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MenuUISelector : MonoBehaviour
{
    public Transform botonesContainer;

    void OnEnable()
    {
        SeleccionarPrimerBotonValido();
    }

    void SeleccionarPrimerBotonValido()
    {
        EventSystem.current.SetSelectedGameObject(null);

        foreach (Transform boton in botonesContainer)
        {
            Button btn = boton.GetComponent<Button>();
            if (btn == null) continue;

            //Ignorar botones ocultos
            if (!boton.gameObject.activeInHierarchy) continue;

            //Ignorar botones no interactuables (grises)
            if (!btn.interactable) continue;

            //Seleccionar primer botón válido
            EventSystem.current.SetSelectedGameObject(boton.gameObject);
            return;
        }
    }
}