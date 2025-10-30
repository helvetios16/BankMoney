using UnityEngine;
using UnityEngine.UI;

public class ResetButtonController : MonoBehaviour
{
    public Button resetButton;
    public ReceptionTrigger receptionTrigger;

    void Start()
    {
        if (resetButton != null)
            resetButton.onClick.AddListener(OnResetButtonClicked);
    }

    public void OnResetButtonClicked()
    {
        receptionTrigger.contadorTransaccion = 0;
        ReceptionTrigger.contadorGlobal = 0;

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
    }
}