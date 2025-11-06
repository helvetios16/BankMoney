using UnityEngine;
using System.Collections;

public class NPCPathFollower : MonoBehaviour
{
    public Transform[] puntos;
    public float velocidad = 0.5f;
    private int indice = 0;
    private int direccion = 1;
    private Animator anim;
    private bool primeraVez = true;

    public CountdownTimer countdownTimer;
    public TransactionGoalController transactionGoal;
    public ToggleButtonEnable toogleButtonEnable;

    public BlinkOnBool blinkOnBool;

    private bool _shouldInterrupt = false;     // bandera de interrupción
    private Coroutine moverCoroutine;

    void Start()
    {
        anim = GetComponent<Animator>();
        moverCoroutine = StartCoroutine(MoverNPC());
    }

    IEnumerator MoverNPC()
    {
        while (true)
        {
            Vector3 destino = puntos[indice].position;

            while (Vector3.Distance(transform.position, destino) > 0.1f)
            {
                transform.position = Vector3.MoveTowards(transform.position, destino, velocidad * Time.deltaTime);
                transform.LookAt(destino);
                yield return null;
            }

            if (indice == puntos.Length - 1) // Último punto
            {
                anim.speed = 0f;

                countdownTimer.StopTimer();
                countdownTimer.ResetTimer();
                transactionGoal.GenerateNewGoal();
                countdownTimer.StartTimer();

                blinkOnBool.StartBlink();

                // Reseteamos la bandera antes de esperar
                _shouldInterrupt = false;

                float waitTime = 30f;
                float elapsed = 0f;
                while (elapsed < waitTime)
                {
                    elapsed += Time.deltaTime;
                    if (_shouldInterrupt) break;
                    yield return null;
                }

                blinkOnBool.StopBlink();

                anim.speed = 1f;
                direccion = -1; // Empieza a retroceder
            }
            else if (indice == 0)
            {
                if (!primeraVez)
                {
                    anim.speed = 0f;
                    yield return new WaitForSeconds(2f);
                    anim.speed = 1f;
                }
                else
                {
                    primeraVez = false;
                }
                direccion = 1;
            }

            indice += direccion;
            indice = Mathf.Clamp(indice, 0, puntos.Length - 1);

            // En cualquier cambio de dirección, también queremos reiniciar la bandera para futuras esperas
            _shouldInterrupt = false;
        }
    }

    public void InterruptAndGoBack()
    {
        _shouldInterrupt = true;
    }
}
