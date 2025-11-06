using UnityEngine;
using System.Collections;

public class BlinkOnBool : MonoBehaviour
{
    [Tooltip("El objeto que parpadeará")]
    public GameObject targetObject;  

    public float blinkInterval = 0.5f;
    public Color blinkColor = Color.green;

    private Color originalColor;
    private Renderer targetRenderer;
    private Coroutine blinkCoroutine;

    void Start()
    {
        if (targetObject == null)
        {
            Debug.LogError("BlinkOnBool: targetObject no está asignado.");
            enabled = false;
            return;
        }

        targetRenderer = targetObject.GetComponent<Renderer>();
        if (targetRenderer == null)
        {
            Debug.LogError("BlinkOnBool: targetObject no tiene componente Renderer.");
            enabled = false;
            return;
        }

        originalColor = targetRenderer.material.color;
    }

    /// <summary>
    /// Método público para iniciar el parpadeo.
    /// </summary>
    public void StartBlink()
    {
        if (blinkCoroutine == null)
        {
            blinkCoroutine = StartCoroutine(BlinkRoutine());
        }
    }

    /// <summary>
    /// Método público para detener el parpadeo y restaurar el color original.
    /// </summary>
    public void StopBlink()
    {
        if (blinkCoroutine != null)
        {
            StopCoroutine(blinkCoroutine);
            blinkCoroutine = null;
        }
        // restaurar color original
        targetRenderer.material.color = originalColor;
    }

    private IEnumerator BlinkRoutine()
    {
        while (true)
        {
            targetRenderer.material.color = blinkColor;
            yield return new WaitForSeconds(blinkInterval);
            targetRenderer.material.color = originalColor;
            yield return new WaitForSeconds(blinkInterval);
        }
    }
}