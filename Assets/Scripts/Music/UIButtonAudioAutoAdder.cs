using UnityEngine;
using UnityEngine.UI;

public class UIButtonAudioAutoAdder : MonoBehaviour
{
    void Start()
    {
        AddAudioToButtons();
    }

    void AddAudioToButtons()
    {
        Button[] botones = FindObjectsOfType<Button>(true);

        foreach (Button btn in botones)
        {
            if (btn.GetComponent<UIButtonAudio>() == null)
            {
                btn.gameObject.AddComponent<UIButtonAudio>();
            }
        }
    }
}
