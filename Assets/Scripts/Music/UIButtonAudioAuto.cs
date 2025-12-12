using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIButtonAudioAuto : MonoBehaviour
{
    void OnEnable()
    {
        StartCoroutine(AsignarAudioATodosLosBotones());
    }

    IEnumerator AsignarAudioATodosLosBotones()
    {
        // Espera 1 frame para permitir que los menús activen botones
        yield return null;

        Button[] botones = GameObject.FindObjectsOfType<Button>(true);

        foreach (Button btn in botones)
        {
            if (!btn.gameObject.activeInHierarchy) continue;

            if (btn.GetComponent<UIButtonAudio>() == null)
            {
                btn.gameObject.AddComponent<UIButtonAudio>();
            }
        }
    }
}
