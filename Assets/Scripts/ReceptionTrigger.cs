using UnityEngine;
using System.Collections.Generic;

public class ReceptionTrigger : MonoBehaviour
{
    public UIContadorController uiController;
    public TransactionGoalController transactionGoalController;
    public CountdownTimer receptionTimer;
    public PersonCounterController personCounterController;
    public NPCPathFollower npcPathFollower;
    public BarraDeDinero barraDeDinero; // ← referencia a tu barra

    public int contadorTransaccion = 0;
    private HashSet<GameObject> objetosProcesados = new HashSet<GameObject>();

    private void Start()
    {
        // Si hay una barra de dinero, inicializamos con 0
        if (barraDeDinero != null)
        {
            barraDeDinero.dineroActual = 0f;
            barraDeDinero.dineroTotal = transactionGoalController != null 
                ? transactionGoalController.GetGoalAmount() 
                : 100f; // valor por defecto si no hay meta asignada
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        GameObject go = other.gameObject;

        if (objetosProcesados.Contains(go))
            return;

        string tag = go.tag;
        int monto = ObtenerMontoDesdeTag(tag);
        if (monto == 0)
            return;

        objetosProcesados.Add(go);
        go.tag = tag + "Procesado";

        contadorTransaccion += monto;
        Debug.Log($"Objeto recibido: {monto} | Total actual: {contadorTransaccion}");

        // Actualiza la UI del contador
        if (uiController != null)
            uiController.ActualizarTransaccion(contadorTransaccion);

        // Actualiza la barra visual
        if (barraDeDinero != null)
            barraDeDinero.dineroActual = contadorTransaccion;

        VerificarObjetivo();
    }

    private int ObtenerMontoDesdeTag(string tag)
    {
        return tag switch
        {
            "Monto10k" => 10000,
            "Monto100" => 100,
            "Monto50" => 50,
            "Monto20" => 20,
            "Monto10" => 10,
            "Monto5" => 5,
            "Monto1" => 1,
            _ => 0
        };
    }

    private void VerificarObjetivo()
    {
        if (transactionGoalController == null)
        {
            Debug.LogError("ReceptionTrigger: transactionGoalController no asignado.");
            return;
        }

        bool objetivoCumplido = transactionGoalController.CheckGoal(contadorTransaccion);

        if (objetivoCumplido)
        {
            Debug.Log("✅ Objetivo alcanzado correctamente.");
            ProcesarResultado(exito: true);
        }
        else
        {
            int meta = transactionGoalController.GetGoalAmount();
            if (contadorTransaccion > meta)
            {
                Debug.Log("❌ Se pasó del objetivo, reiniciando...");
                ProcesarResultado(exito: false);
            }
        }
    }

    private void ProcesarResultado(bool exito)
    {
        MontoItem[] items = Object.FindObjectsByType<MontoItem>(FindObjectsSortMode.None);
        foreach (MontoItem item in items)
            item.ResetItem();

        if (uiController != null)
            uiController.ActualizarTransaccion(0);

        contadorTransaccion = 0;
        objetosProcesados.Clear();

        // Reinicia la barra de dinero
        if (barraDeDinero != null)
            barraDeDinero.dineroActual = 0f;

        if (receptionTimer != null)
        {
            receptionTimer.StopTimer();
            receptionTimer.ResetTimer();
        }

        if (exito)
        {
            if (personCounterController != null)
                personCounterController.AddPerson();

            if (npcPathFollower != null)
                npcPathFollower.InterruptAndGoBack();
        }
    }
}
