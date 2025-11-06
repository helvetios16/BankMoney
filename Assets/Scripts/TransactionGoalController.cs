using UnityEngine;
using TMPro;

public class TransactionGoalController : MonoBehaviour
{
    public TMP_Text goalText;  // UI para mostrar el objetivo
    private int goalAmount = 0;

    // valores permitidos
    private readonly int[] possibleAmounts = { 100, 10000 };
    
    public PersonCounterController personCounterController;

    void Start()
    {
        // GenerateNewGoal();
    }

    public void GenerateNewGoal()
    {
        // elegir monto aleatorio entre los posibles
        int choiceIndex = Random.Range(0, possibleAmounts.Length);
        int baseAmount = possibleAmounts[choiceIndex];

        goalAmount = baseAmount;
        UpdateGoalUI();
        Debug.Log("Nuevo objetivo generado: " + goalAmount);
    }

    private void UpdateGoalUI()
    {
        if (goalText != null)
            goalText.text = "$" + goalAmount.ToString() + " /";
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