using UnityEngine;
using TMPro;  // si usas TextMeshPro

public class CountdownTimer : MonoBehaviour
{
    public float timeRemaining = 30f;     // 30 segundos
    public bool timerIsRunning = false;

    public TMP_Text timeText;             // arrastra aquí tu componente de texto

    private void Start()
    {
        // empieza el contador
        timerIsRunning = true;
        UpdateTimeText(timeRemaining);
    }

    private void Update()
    {
        if (timerIsRunning)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
                UpdateTimeText(timeRemaining);
            }
            else
            {
                timeRemaining = 0;
                timerIsRunning = false;
                UpdateTimeText(timeRemaining);
                OnTimerEnd();
            }
        }
    }

    private void UpdateTimeText(float timeToDisplay)
    {
        // Mostrar como “00:30”, “00:29”, etc
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);
        timeText.text = string.Format("{0:00}", seconds);
    }

    private void OnTimerEnd()
    {
        Debug.Log("¡Tiempo terminado!");
        // Aquí llama al método ShowLose() o ShowWin() según corresponda
        // Por ejemplo:
        // gameEndController.ShowLose();
    }
}