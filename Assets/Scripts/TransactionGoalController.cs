using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System.Linq;

public class TransactionGoalController : MonoBehaviour
{
    public TMP_Text goalText;  // UI para mostrar el objetivo
    private int goalAmount = 0;

    private readonly int[] possibleAmounts = { 10000, 100, 50, 20, 10, 5, 1 };
    private Dictionary<int, int> amountUsage;

    public int maxRepetitions = 5;

    public PersonCounterController personCounterController;

    // 🔹 Referencia a la barra de dinero
    public BarraDeDinero barraDeDinero;

    void Awake()
    {
        amountUsage = new Dictionary<int, int>();
        foreach (int amount in possibleAmounts)
            amountUsage[amount] = 0;
    }

    void Start()
    {
        GenerateNewGoal();
    }

    public void GenerateNewGoal()
    {
        int maxTargetValue = 500;
        int minTargetValue = 10;
        int targetGoal = Random.Range(minTargetValue, maxTargetValue);

        goalAmount = 0;
        int remainingValue = targetGoal;

        var orderedAmounts = possibleAmounts.OrderByDescending(a => a);

        for (int i = 0; i < 5; i++)
        {
            List<int> available = new List<int>();

            foreach (int amount in orderedAmounts)
            {
                if (amountUsage[amount] < maxRepetitions && remainingValue >= amount)
                    available.Add(amount);
            }

            if (available.Count > 0)
            {
                int chosenAmount = available[Random.Range(0, available.Count)];
                goalAmount += chosenAmount;
                remainingValue -= chosenAmount;
                amountUsage[chosenAmount]++;
            }
            else break;

            if (remainingValue < 5 && remainingValue > 0)
            {
                if (amountUsage.ContainsKey(remainingValue) && amountUsage[remainingValue] < maxRepetitions)
                {
                    goalAmount += remainingValue;
                    amountUsage[remainingValue]++;
                }
                break;
            }

            if (remainingValue <= 0) break;
        }

        if (goalAmount == 0)
        {
            goalAmount = Random.Range(10, 50);
            Debug.LogWarning("Objetivo forzado debido a limitaciones de billetes.");
        }

        UpdateGoalUI();

        // 🔹 Enviar el total a la barra de dinero
        if (barraDeDinero != null)
        {
            barraDeDinero.SetDineroTotal(goalAmount);
            barraDeDinero.ReiniciarBarra();
            barraDeDinero.ActualizarBarra();
        }
        else
        {
            Debug.LogWarning("⚠️ TransactionGoalController: No se asignó la referencia a BarraDeDinero en el Inspector.");
        }

        Debug.Log($"Nuevo objetivo generado: {goalAmount} (Target inicial: {targetGoal})");
    }

    private void UpdateGoalUI()
    {
        if (goalText != null)
        {
            goalText.text = "$" + goalAmount.ToString();
        }
        else
        {
            Debug.LogError("TransactionGoalController: goalText no asignado en Inspector.");
        }
    }

    public bool CheckGoal(int transactionAmount)
    {
        if (transactionAmount == goalAmount)
        {
            Debug.Log("✅ ¡Objetivo alcanzado!");
            return true;
        }
        else
        {
            Debug.Log($"❌ Objetivo no alcanzado. Has: {transactionAmount} | Objetivo: {goalAmount}");
            return false;
        }
    }

    public void ResetGoal()
    {
        foreach (int amount in possibleAmounts)
            amountUsage[amount] = 0;

        GenerateNewGoal();
    }

    public int GetGoalAmount()
    {
        return goalAmount;
    }
}
