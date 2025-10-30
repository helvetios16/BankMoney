using UnityEngine;
using TMPro;

public class PersonCounterController : MonoBehaviour
{
    public int maxPersons = 5;
    public int currentPersons = 1;

    public GameEndController gameEndController;

    public TMP_Text personsText; // texto UI para mostrar “current/max”

    void Start()
    {
        UpdatePersonsUI();
    }

    public void AddPerson()
    {
        if (currentPersons < maxPersons)
        {
            currentPersons++;
            UpdatePersonsUI();
            CheckEndCondition();
        }
        else
        {
            Debug.Log("Ya alcanzaste el máximo: " + currentPersons + "/" + maxPersons);
        }
    }

    public void RemovePerson()
    {
        if (currentPersons > 0)
        {
            currentPersons--;
            UpdatePersonsUI();
            CheckEndCondition();
        }
        else
        {
            Debug.Log("Ya estás en cero personas.");
        }
    }

    private void UpdatePersonsUI()
    {
        if (personsText != null)
        {
            personsText.text = currentPersons + "/" + maxPersons;
        }
        else
        {
            Debug.LogError("PersonCounterController: personsText no asignado en Inspector.");
        }
    }

    private void CheckEndCondition()
    {
        if (currentPersons >= maxPersons)
        {
            if (gameEndController != null)
                gameEndController.ShowWin();
            else
                Debug.LogError("PersonCounterController: gameEndController no asignado.");
        }
        else if (currentPersons <= 0)
        {
            if (gameEndController != null)
                gameEndController.ShowLose();
            else
                Debug.LogError("PersonCounterController: gameEndController no asignado.");
        }
    }
}