using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

public class PasoInteractionUI : MonoBehaviour
{
    public PasoController paso;           // Referencia al PasoController
    public GameObject interactionUI;      // Texto flotante
    public string keyboardKey = "E";
    public string gamepadButton = "X";    // Botón del mando

    private TextMeshProUGUI textUI;

    void Start()
    {
        if (interactionUI != null)
        {
            interactionUI.SetActive(false);
            textUI = interactionUI.GetComponent<TextMeshProUGUI>();
            if (textUI == null)
            {
                Debug.LogWarning("PasoInteractionUI: No se encontró TextMeshProUGUI en interactionUI.");
            }
        }
        else
        {
            Debug.LogWarning("PasoInteractionUI: interactionUI no asignado.");
        }
    }

    void Update()
    {
        if (paso == null || textUI == null) return;

        // Mostrar el texto solo si el jugador está cerca y el paso puede interactuar
        if (paso.jugadorCerca && !paso.Animado && !paso.FullDrainPenaltyActive && paso.CurrentStamina > 0)
        {
            UpdateText();
            interactionUI.SetActive(true);
        }
        else
        {
            interactionUI.SetActive(false);
        }
    }

    void LateUpdate()
    {
        if (interactionUI.activeSelf)
        {
            interactionUI.transform.localScale =
                Vector3.one * (1f + Mathf.Sin(Time.time * 4f) * 0.05f);
        }
    }

    void UpdateText()
    {
        bool usingGamepad = Gamepad.current != null;
        string boton = usingGamepad ? gamepadButton : keyboardKey;

        if (paso.Levantado)
            textUI.text = $"Pulsa <b>{boton}</b> para bajar";
        else
            textUI.text = $"Pulsa <b>{boton}</b> para levantar";
    }
}
