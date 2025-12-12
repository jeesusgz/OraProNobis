using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIButtonAudio : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler, ISubmitHandler
{
    private Button boton;

    void Awake()
    {
        boton = GetComponent<Button>();
        if (boton == null)
            Debug.LogError("[UIButtonAudio] No se encontró un componente Button.");
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (boton != null && boton.interactable)
            UIAudioManager.Instance.PlayHover();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (boton == null) return;

        if (boton.interactable)
            UIAudioManager.Instance.PlayClick();
        else
            UIAudioManager.Instance.PlayError(); 
    }

    public void OnSubmit(BaseEventData eventData)
    {
        if (boton == null) return;

        if (boton.interactable)
            UIAudioManager.Instance.PlayClick();
        else
            UIAudioManager.Instance.PlayError();
    }
}