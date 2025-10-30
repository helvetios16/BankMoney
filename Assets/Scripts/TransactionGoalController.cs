using UnityEngine;
using TMPro;

public class TransactionGoalController : MonoBehaviour
{
    public TMP_Text goalText;  // UI para mostrar el objetivo
    public int maxAttempts = 3;

    private int goalAmount = 0;
    private int attemptsLeft;

    // valores permitidos
    private readonly int[] possibleAmounts = { 100, 10000 };
    
    public PersonCounterController personCounterController;

    void Start()
    {
        GenerateNewGoal();
    }

    public void GenerateNewGoal()
    {
        // elegir monto aleatorio entre los posibles
        int choiceIndex = Random.Range(0, possibleAmounts.Length);
        int baseAmount = possibleAmounts[choiceIndex];

        // generar cuántas veces (1 a maxAttempts)
        int times = Random.Range(1, maxAttempts + 1);

        goalAmount = baseAmount * times;
        attemptsLeft = maxAttempts;

        UpdateGoalUI();
        Debug.Log("Nuevo objetivo generado: " + goalAmount);
    }

    private void UpdateGoalUI()
    {
        if (goalText != null)
            goalText.text = "Objetivo: $" + goalAmount.ToString() +
                            "\nIntentos restantes: " + attemptsLeft.ToString();
    }


    // Método para verificar si la transacción coincide
    public bool CheckGoal(int transactionAmount)
    {
        if (transactionAmount == goalAmount)
        {
            Debug.Log("¡Objetivo alcanzado!");
            return true;
        }
        else
        {
            Debug.Log("Objetivo no alcanzado. Has: " + transactionAmount + " | Objetivo: " + goalAmount);
            return false;
        }
    }

    public void MinusAttempt()
    {
        if (attemptsLeft > 0)
        {
            attemptsLeft--;
            UpdateGoalUI();

            if (attemptsLeft == 0)
            {
                Debug.Log("Ya no quedan intentos.");
                // Aquí puedes llamar a la función de perder, por ejemplo:
                // gameEndController.ShowLose();
                personCounterController.RemovePerson();
            }
        }
        else
        {
            Debug.LogWarning("Intentos ya en 0, no se puede disminuir más.");
        }
    }


    // Método público para llamar cuando termina el juego o se reinicia
    public void ResetGoal()
    {
        GenerateNewGoal();
    }

    public int GetGoalAmount()
    {
        return goalAmount;
    }
}