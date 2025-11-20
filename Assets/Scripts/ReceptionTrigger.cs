using UnityEngine;
using System.Collections.Generic;

public class ReceptionTrigger : MonoBehaviour
{
    [Header("Referencias principales")]
    public UIContadorController uiController;
    public TransactionGoalController transactionGoalController;
    public CountdownTimer receptionTimer;
    public PersonCounterController personCounterController;
    public NPCPathFollower npcPathFollower;
    public BarraDeDinero barraDeDinero;

    [Header("Estado interno")]
    public int contadorTransaccion = 0;
    private HashSet<GameObject> objetosProcesados = new HashSet<GameObject>();

    private void Awake()
    {
        // 🔍 Diagnóstico de referencias
        if (barraDeDinero == null)
            Debug.LogError("❌ [ReceptionTrigger] Falta asignar la referencia 'BarraDeDinero' en el inspector.");

        if (transactionGoalController == null)
            Debug.LogError("❌ [ReceptionTrigger] Falta asignar la referencia 'TransactionGoalController' en el inspector.");

        if (uiController == null)
            Debug.LogWarning("⚠️ [ReceptionTrigger] No se asignó 'UIContadorController'. No se mostrará el contador visual.");

        if (npcPathFollower == null)
            Debug.LogWarning("⚠️ [ReceptionTrigger] No se asignó 'NPCPathFollower'. El NPC no se moverá al cumplir el objetivo.");
    }

    private void Start()
    {
        // ✅ Inicializa la barra con total desde el objetivo
        if (barraDeDinero != null)
        {
            barraDeDinero.dineroActual = 0f;

            if (transactionGoalController != null)
                barraDeDinero.SetDineroTotal(transactionGoalController.GetGoalAmount());
            else
                barraDeDinero.SetDineroTotal(100f);

            barraDeDinero.ActualizarBarra();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"👉 OnTriggerEnter detectado con: {other.name} ({other.tag})");

        if (barraDeDinero == null || transactionGoalController == null)
        {
            Debug.LogError("🚫 ReceptionTrigger: Faltan referencias (BarraDeDinero o TransactionGoalController). No se puede continuar.");
            return;
        }

        GameObject go = other.gameObject;

        if (objetosProcesados.Contains(go))
            return;

        string tag = go.tag;
        int monto = ObtenerMontoDesdeTag(tag);
        if (monto == 0)
        {
            Debug.Log($"⚠️ Etiqueta {tag} no tiene monto asignado.");
            return;
        }

        objetosProcesados.Add(go);
        go.tag = tag + "Procesado";

        contadorTransaccion += monto;
        Debug.Log($"💵 Objeto recibido: {monto} | Total actual: {contadorTransaccion}");

        // 🧮 Actualiza el contador visual
        if (uiController != null)
            uiController.ActualizarTransaccion(contadorTransaccion);

        // 💰 Solo actualiza el dinero actual en la barra
        if (barraDeDinero != null)
        {
            barraDeDinero.SetDineroActual(contadorTransaccion);
            barraDeDinero.ActualizarBarra();
        }

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
        int meta = transactionGoalController.GetGoalAmount();

        if (objetivoCumplido)
        {
            Debug.Log("✅ Objetivo alcanzado correctamente.");
            ProcesarResultado(true);
        }
        else if (contadorTransaccion > meta)
        {
            Debug.Log("⚠️ Se pasó del objetivo. Reiniciando barra y contador (sin afectar el tiempo).");
            ProcesarResultado(false);
        }
    }

    private void ProcesarResultado(bool exito)
    {
        // 🔄 Reinicia ítems visuales
        MontoItem[] items = Object.FindObjectsByType<MontoItem>(FindObjectsSortMode.None);
        foreach (MontoItem item in items)
            item.ResetItem();

        // 🧮 Reinicia contador visual
        if (uiController != null)
            uiController.ActualizarTransaccion(0);

        contadorTransaccion = 0;
        objetosProcesados.Clear();

        // 💰 Reinicia barra solo el dinero actual
        if (barraDeDinero != null)
        {
            barraDeDinero.SetDineroActual(0f);
            barraDeDinero.ActualizarBarra();
        }

        // ⏱️ Solo reinicia tiempo si fue éxito
        if (exito && receptionTimer != null)
        {
            receptionTimer.StopTimer();
            receptionTimer.ResetTimer();
        }

        // 🚶 Si fue éxito → NPC se va y se suma persona
        if (exito)
        {
            if (personCounterController != null)
                personCounterController.AddPerson();

            if (npcPathFollower != null)
                npcPathFollower.InterruptAndGoBack();
        }
    }
}
