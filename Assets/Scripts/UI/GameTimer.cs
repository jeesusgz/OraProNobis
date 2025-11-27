using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement; // Opcional, para reiniciar escena

public class GameTimer : MonoBehaviour
{
    public float totalTime = 600f; // Tiempo en segundos
    public TMP_Text timerText;    // Asignar en Inspector
    private float currentTime;
    private bool isRunning = true;

    void Start()
    {
        currentTime = totalTime;
        UpdateUI();
    }

    void Update()
    {
        if (!isRunning) return;

        currentTime -= Time.deltaTime;

        if (currentTime <= 0)
        {
            currentTime = 0;
            isRunning = false;
            TimerFinished();
        }

        if (currentTime <= 60f)
            timerText.color = Color.red;

        UpdateUI();
    }

    void UpdateUI()
    {
        if (timerText != null)
        {
            int minutes = Mathf.FloorToInt(currentTime / 60f);
            int seconds = Mathf.FloorToInt(currentTime % 60f);
            timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }
    }

    void TimerFinished()
    {
        Debug.Log("¡Se acabó el tiempo! Game Over");
        // Aquí puedes activar tu pantalla de Game Over
        // Ejemplo: cargar escena de Game Over
        // SceneManager.LoadScene("GameOverScene");
    }
}
