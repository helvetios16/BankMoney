using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System.Linq; // <--- ¡Esta es la línea que faltaba y soluciona el error!

public class TransactionGoalController : MonoBehaviour
{
    public TMP_Text goalText;  // UI para mostrar el objetivo
    private int goalAmount = 0;

    // Billetes base permitidos para construir el monto objetivo.
    private readonly int[] possibleAmounts = { 10000, 100, 50, 20, 10, 5, 1 }; 

    // Diccionario para llevar cuenta de uso de CADA BILLETE base
    private Dictionary<int, int> amountUsage;

    public int maxRepetitions = 5;  // Máximo de veces que un BILLETE base puede usarse

    public PersonCounterController personCounterController;

    void Awake()
    {
        // Inicializar el diccionario de uso de billetes base
        amountUsage = new Dictionary<int, int>();
        foreach (int amount in possibleAmounts)
        {
            amountUsage[amount] = 0;
        }
    }

    void Start()
    {
        GenerateNewGoal();
    }

    public void GenerateNewGoal()
    {
        // 1. Determinar el Rango del Objetivo
        // Puedes ajustar estos límites según la dificultad.
        int maxTargetValue = 500;
        int minTargetValue = 10;
        
        // Genera un valor aleatorio entre los límites
        int targetGoal = Random.Range(minTargetValue, maxTargetValue);
        
        // 2. Construir el Monto Objetivo con Control de Repetición

        goalAmount = 0;
        int remainingValue = targetGoal;
        
        // Ordenamos los montos de mayor a menor para una construcción eficiente.
        var orderedAmounts = possibleAmounts.OrderByDescending(a => a);

        // Repetimos la construcción un número de veces (limitado)
        for (int i = 0; i < 5; i++)
        {
            // En cada iteración, elegimos aleatoriamente un BILLETE DISPONIBLE
            
            List<int> available = new List<int>();
            
            // Llenar la lista 'available' con billetes que no hayan excedido su límite de uso
            foreach (int amount in orderedAmounts)
            {
                // Solo considera los billetes que son menores o iguales al valor restante
                // Y que no hayan excedido el máximo de repeticiones.
                if (amountUsage[amount] < maxRepetitions && remainingValue >= amount)
                {
                    available.Add(amount);
                }
            }

            // Si quedan billetes disponibles para usar en este ciclo
            if (available.Count > 0)
            {
                // Elegimos aleatoriamente uno de los billetes disponibles
                int choiceIndex = Random.Range(0, available.Count);
                int chosenAmount = available[choiceIndex];

                // Usar el billete para el objetivo
                goalAmount += chosenAmount;
                remainingValue -= chosenAmount;

                // Aumentar el contador de uso para ese billete base
                amountUsage[chosenAmount]++;
            }
            else
            {
                // Si no hay billetes disponibles, salir.
                break;
            }

            // Opcional: si el valor restante es muy bajo, salir del loop para no usar solo billetes de $1
            if (remainingValue < 5 && remainingValue > 0)
            {
                // Solo añade lo que queda si es un billete base y no excede la repetición
                if (amountUsage.ContainsKey(remainingValue) && amountUsage[remainingValue] < maxRepetitions)
                {
                    goalAmount += remainingValue;
                    amountUsage[remainingValue]++;
                }
                break;
            }

            // Si el monto restante es 0 o negativo, salimos
            if (remainingValue <= 0) break;
        }

        // Si el objetivo final es 0, forzar uno mínimo
        if (goalAmount == 0)
        {
            goalAmount = Random.Range(10, 50); 
            Debug.LogWarning("Objetivo forzado debido a la limitación de billetes o rango.");
        }

        UpdateGoalUI();
        Debug.Log($"Nuevo objetivo generado: {goalAmount} (Target inicial: {targetGoal})");
    }

    private void UpdateGoalUI()
    {
        if (goalText != null)
        {
            goalText.text = "$" + goalAmount.ToString() + "";
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
            Debug.Log("¡Objetivo alcanzado!");
            return true;
        }
        else
        {
            Debug.Log($"Objetivo no alcanzado. Has: {transactionAmount} | Objetivo: {goalAmount}");
            return false;
        }
    }

    public void ResetGoal()
    {
        // Reiniciar los usos
        foreach (int amount in possibleAmounts)
        {
            amountUsage[amount] = 0;
        }
        GenerateNewGoal();
    }

    public int GetGoalAmount()
    {
        return goalAmount;
    }
}