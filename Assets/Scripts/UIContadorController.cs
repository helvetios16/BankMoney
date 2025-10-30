using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIContadorController : MonoBehaviour
{
    public TMP_Text textoTransaccion;

    void Start()
    {
        if (textoTransaccion == null)
        {
            Debug.LogError("UIContadorController: textoTransaccion no está asignado en el Inspector");
            return;
        }
        textoTransaccion.text = "Transacción: ";
    }


    public void ActualizarTransaccion(int monto)
    {
        textoTransaccion.text = "Transacción: \n$" + monto.ToString();
    }
}

