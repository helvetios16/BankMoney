using UnityEngine;
using TMPro;

public class CountdownTimer : MonoBehaviour
{
    public float initialTime = 30f;          // Tiempo inicial que quieres
    private float timeRemaining;
    public bool timerIsRunning = false;
    public TMP_Text timeText;
    
    public PersonCounterController personCounterController;

    void Start()
    {
        ResetTimer();                          // Al comenzar, lo inicializamos
    }

    void Update()
    {
        if (timerIsRunning)
        {
            if (timeRemaining > 0f)
            {
                timeRemaining -= Time.deltaTime;
                if (timeRemaining < 0f)  // añadido
                {
                    timeRemaining = 0f;
                }
                UpdateTimeText(timeRemaining);
            }
            else
            {
                timeRemaining = 0f;
                UpdateTimeText(timeRemaining);
                OnTimerEnd();
                Debug.Log("🕒 Timer finalizado — llamando a RemovePerson()");
                timerIsRunning = false;
            }

        }
    }



    private void UpdateTimeText(float timeToDisplay)
    {
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);
        timeText.text = string.Format("{0:00}", seconds);
    }

    private void OnTimerEnd()
    {
        Debug.Log("¡Tiempo terminado!");

        if (personCounterController != null)
            personCounterController.RemovePerson();
        else
            Debug.LogError("CountdownTimer: personCounterController no asignado en el Inspector.");
    }


    // Función pública para reiniciar el contador
    public void ResetTimer()
    {
        timeRemaining = initialTime;
        UpdateTimeText(timeRemaining);
    }

    // Función pública para comenzar el contador
    public void StartTimer()
    {
        timerIsRunning = true;
    }

    // Función pública para detener el contador
    public void StopTimer()
    {
        timerIsRunning = false;
    }
}