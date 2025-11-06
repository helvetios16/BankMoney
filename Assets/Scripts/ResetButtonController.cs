using UnityEngine;
using UnityEngine.UI;

public class ResetButtonController : MonoBehaviour
{
    public Button resetButton;
    public ReceptionTrigger receptionTrigger;
    
    public GameEndController gameEndController;
    
    public CountdownTimer receptionTimer;
    
    public TransactionGoalController transactionGoalController;
    
    public PersonCounterController personCounterController;

    void Start()
    {
        if (resetButton != null)
            resetButton.onClick.AddListener(OnResetButtonClicked);
    }

    public void OnResetButtonClicked()
    {
        // Comprobamos el objetivo
        if (transactionGoalController.CheckGoal(receptionTrigger.contadorTransaccion))
        {
            // Se alcanzó el objetivo
            receptionTrigger.contadorTransaccion = 0;

            MontoItem[] items = Object.FindObjectsByType<MontoItem>(FindObjectsSortMode.None);
            foreach (MontoItem item in items)
            {
                item.ResetItem();
            }

            if (receptionTrigger.uiController != null)
            {
                receptionTrigger.uiController.ActualizarTransaccion(0);
            }
            else
            {
                Debug.LogError("ResetButtonController: uiController en receptionTrigger no está asignado.");
            }

            // Finalización o comportamiento de éxito
            // gameEndController.ShowLose(); // O ShowWin() si lo tienes para éxito

            receptionTimer.StopTimer();
            receptionTimer.ResetTimer();

            personCounterController.AddPerson();
        }
        else
        {
            // No se alcanzó el objetivo
            receptionTrigger.contadorTransaccion = 0;

            MontoItem[] items = Object.FindObjectsByType<MontoItem>(FindObjectsSortMode.None);
            foreach (MontoItem item in items)
            {
                item.ResetItem();
            }

            if (receptionTrigger.uiController != null)
            {
                receptionTrigger.uiController.ActualizarTransaccion(0);
            }
            else
            {
                Debug.LogError("ResetButtonController: uiController en receptionTrigger no está asignado.");
            }

            // Aquí no se hace MinusAttempt ni se maneja intentos
            // Puedes añadir otra lógica para fallo si lo deseas
        }
    }
}
