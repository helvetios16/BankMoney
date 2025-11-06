using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PersonCounterController : MonoBehaviour
{
    public int maxPersons = 10;
    public int currentPersons = 2;

    public GameEndController gameEndController;

    public TMP_Text personsText; // texto UI para mostrar “current/max”

    // --- Nuevo campo para el slider visual ---
    public Slider personsSlider;            // referencia al Slider
    public Image fillImage;                // la imagen de “Fill” del Slider
    public Color colorMin = Color.red;     // color para 0 personas
    public Color colorMax = Color.green;   // color para maxPersons

    void Start()
    {
        if (personsSlider != null)
        {
            personsSlider.minValue = 0;
            personsSlider.maxValue = maxPersons;
            personsSlider.wholeNumbers = true;
        }
        UpdatePersonsUI();
        UpdateSliderUI();
    }

    public void AddPerson()
    {
        if (currentPersons < maxPersons)
        {
            currentPersons++;
            UpdatePersonsUI();
            UpdateSliderUI();
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
            UpdateSliderUI();
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

    private void UpdateSliderUI()
    {
        if (personsSlider != null)
        {
            personsSlider.value = currentPersons;

            if (fillImage != null)
            {
                float t = (float)currentPersons / (float)maxPersons;
                fillImage.color = Color.Lerp(colorMin, colorMax, t);
            }
            else
            {
                Debug.LogWarning("PersonCounterController: fillImage no asignado en Inspector.");
            }
        }
        else
        {
            Debug.LogWarning("PersonCounterController: personsSlider no asignado en Inspector.");
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
