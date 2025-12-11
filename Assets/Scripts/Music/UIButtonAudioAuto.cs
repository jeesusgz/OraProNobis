using UnityEngine;
using UnityEngine.UI;

public class UIButtonAudioAuto : MonoBehaviour
{
    void Start()
    {
        Button[] botones = GameObject.FindObjectsOfType<Button>();
        foreach (Button btn in botones)
        {
            if (btn.GetComponent<UIButtonAudio>() == null)
                btn.gameObject.AddComponent<UIButtonAudio>();
        }
    }
}
