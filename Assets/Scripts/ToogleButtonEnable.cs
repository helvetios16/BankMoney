using UnityEngine;
using UnityEngine.UI;

public class ToggleButtonEnable : MonoBehaviour
{
    [Tooltip("El componente Button al que quieres controlar")]
    public Button targetButton;

    [Tooltip("El color que tendrá cuando esté deshabilitado")]
    public Color disabledColor = Color.grey;

    private Color originalColor;
    private Image targetImage;  // suponemos que el botón tiene un componente Image para la parte visual
    private ColorBlock originalColors;  // guardamos los colores del Button

    void Awake()
    {
        if (targetButton == null)
        {
            Debug.LogError("ToggleButtonEnable: targetButton no asignado.");
            enabled = false;
            return;
        }

        targetImage = targetButton.GetComponent<Image>();
        if (targetImage == null)
        {
            Debug.LogError("ToggleButtonEnable: targetButton no tiene componente Image.");
            enabled = false;
            return;
        }

        // Guardamos el color original de la imagen
        originalColor = targetImage.color;

        // Guardamos los ColorBlock originales del botón (incluye disabledColor, etc)
        originalColors = targetButton.colors;
    }

    /// <summary>
    /// Activa el botón: lo hace interactivo y pone su color original.
    /// </summary>
    public void EnableButton()
    {
        targetButton.interactable = true;

        // Restauramos el color de la imagen
        targetImage.color = originalColor;

        // Restauramos los colores del botón
        targetButton.colors = originalColors;
    }

    /// <summary>
    /// Desactiva el botón: lo hace no interactivo y pone el color de “deshabilitado”.
    /// </summary>
    public void DisableButton()
    {
        targetButton.interactable = false;

        // Cambiamos el color de la imagen al color de deshabilitado
        targetImage.color = disabledColor;

        // Además podemos ajustar el ColorBlock disabledColor para consistencia
        ColorBlock cb = targetButton.colors;
        cb.disabledColor = disabledColor;
        targetButton.colors = cb;
    }

    void Start()
    {
        // DisableButton();
    }
}
