using UnityEngine;
using System.Collections.Generic;

public class ReceptionTrigger : MonoBehaviour
{
    public UIContadorController uiController;

    public int contadorTransaccion = 0;
    public static int contadorGlobal = 0;

    // Un conjunto para registrar los objetos ya procesados
    private HashSet<GameObject> objetosProcesados = new HashSet<GameObject>();

    private void OnTriggerEnter(Collider other)
    {
        GameObject go = other.gameObject;

        // Si ya lo procesamos antes, salimos
        if (objetosProcesados.Contains(go))
        {
            return;
        }

        string tag = go.tag;
        int monto = 0;

        switch (tag)
        {
            case "Monto10k":
                monto = 10000;
                break;
            case "Monto100":
                monto = 100;
                break;
            case "Monto50":
                monto = 50;
                break;
            case "Monto20":
                monto = 20;
                break;
            case "Monto10":
                monto = 10;
                break;
            case "Monto5":
                monto = 5;
                break;
            case "Monto1":
                monto = 1;
                break;
            default:
                // No es un monto reconocido → salimos
                return;
        }

        // Ahora lo marcamos como procesado
        objetosProcesados.Add(go);

        // Opcionalmente podemos cambiar tag para clarificar
        go.tag = tag + "Procesado";

        contadorTransaccion += monto;
        Debug.Log("Objeto recibido: " + monto);
        Debug.Log("Contador transaccion = " + contadorTransaccion);

        if (uiController != null)
        {
            uiController.ActualizarTransaccion(contadorTransaccion);
        }
        else
        {
            Debug.LogError("ReceptionTrigger: uiController no está asignado en el Inspector.");
        }
    }
}