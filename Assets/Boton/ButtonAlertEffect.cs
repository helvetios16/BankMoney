using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ButtonAlertEffect : MonoBehaviour
{
    private RectTransform rectTransform;
    private Image image;
    private Coroutine effectCoroutine;

    // Tamaños del parpadeo
    private Vector3 normalScale = new Vector3(0.2149f, 0.25f, 1f);
    private Vector3 alertScale = new Vector3(0.25f, 0.29f, 1f);

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        image = GetComponent<Image>();
    }

    // 🔹 Inicia el efecto de alerta
    public void StartAlert()
    {
        if (effectCoroutine != null)
            StopCoroutine(effectCoroutine);

        effectCoroutine = StartCoroutine(AlertEffect());
    }

    // 🔹 Detiene el efecto
    public void StopAlert()
    {
        if (effectCoroutine != null)
            StopCoroutine(effectCoroutine);

        effectCoroutine = null;
        rectTransform.localScale = normalScale;
        image.color = Color.red;
    }

    private IEnumerator AlertEffect()
    {
        while (true)
        {
            // Alterna el color entre rojo y blanco
            image.color = (image.color == Color.red) ? Color.white : Color.red;

            // Agranda un poco el botón y lo vuelve al tamaño normal
            rectTransform.localScale = alertScale;
            yield return new WaitForSeconds(0.2f);
            rectTransform.localScale = normalScale;
            yield return new WaitForSeconds(0.2f);
        }
    }
}