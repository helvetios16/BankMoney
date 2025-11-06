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

    // Nueva referencia al NPC follower
    public NPCPathFollower npcPathFollower;

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

            receptionTimer.StopTimer();
            receptionTimer.ResetTimer();

            personCounterController.AddPerson();

            // Aquí llamamos al NPC para que se vaya inmediatamente
            if (npcPathFollower != null)
            {
                npcPathFollower.InterruptAndGoBack();
            }
            else
            {
                Debug.LogError("ResetButtonController: npcPathFollower no asignado.");
            }
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
        }
    }
}
