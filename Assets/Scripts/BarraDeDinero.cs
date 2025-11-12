using UnityEngine;
using UnityEngine.UI;

public class BarraDeDinero : MonoBehaviour
{
    public Image barraDeDinero;
    public float dineroTotal;
    public float dineroActual;

    void Start()
    {
        dineroActual = 0f;
        if (barraDeDinero != null)
            barraDeDinero.fillAmount = 0f;
    }

    void Update()
    {
        if (barraDeDinero != null && dineroTotal > 0)
            barraDeDinero.fillAmount = dineroActual / dineroTotal;
    }

    public void SetDineroTotal(float total)
    {
        dineroTotal = total;
    }

    public void SetDineroActual(float actual)
    {
        dineroActual = actual;
    }

    public void ReiniciarBarra()
    {
        dineroActual = 0;
    }
}