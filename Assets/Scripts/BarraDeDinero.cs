using UnityEngine;
using UnityEngine.UI;

public class BarraDeDinero : MonoBehaviour
{
    [Header("Referencia a la imagen de la barra (tipo Filled)")]
    public Image barraDeDinero;

    [Header("Valores de dinero")]
    public float dineroTotal = 100f;
    public float dineroActual = 0f;

    void Start()
    {
        ActualizarBarra();
    }

    public void SetDineroTotal(float total)
    {
        dineroTotal = total;
        ActualizarBarra();
    }

    public void SetDineroActual(float actual)
    {
        dineroActual = Mathf.Clamp(actual, 0, dineroTotal);
        ActualizarBarra();
    }

    public void ReiniciarBarra()
    {
        dineroActual = 0;
        ActualizarBarra();
    }

    public void ActualizarBarra()
    {
        if (barraDeDinero == null)
        {
            Debug.LogWarning("⚠️ BarraDeDinero: No hay imagen asignada en el inspector.");
            return;
        }

        if (dineroTotal <= 0)
        {
            Debug.LogWarning("⚠️ BarraDeDinero: dineroTotal es 0 o negativo.");
            barraDeDinero.fillAmount = 0f;
            return;
        }

        barraDeDinero.fillAmount = dineroActual / dineroTotal;
    }
}
