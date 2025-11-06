using UnityEngine;
using System.Collections;

public class NPCPathFollower : MonoBehaviour
{
    public Transform[] puntos;  // Puntos del camino
    public float velocidad = 0.5f; // Velocidad de movimiento
    private int indice = 0;
    private int direccion = 1; // 1 = hacia adelante, -1 = hacia atrás
    private Animator anim;
    private bool primeraVez = true; // Para controlar la primera llegada al punto 0

    public CountdownTimer countdownTimer;
    public TransactionGoalController transactionGoal;
    public  ToggleButtonEnable toogleButtonEnable;

    void Start()
    {
        anim = GetComponent<Animator>();
        StartCoroutine(MoverNPC());
    }

    IEnumerator MoverNPC()
    {
        while (true)
        {
            Vector3 destino = puntos[indice].position;

            // Moverse hacia el punto actual
            while (Vector3.Distance(transform.position, destino) > 0.1f)
            {
                transform.position = Vector3.MoveTowards(transform.position, destino, velocidad * Time.deltaTime);
                transform.LookAt(destino);
                yield return null;
            }

            // Llegó al punto → detener animación y esperar 10s solo si es el primero o el último
            if (indice == puntos.Length - 1) // Último punto
            {
                anim.speed = 0f;

                countdownTimer.StopTimer();  // 🔥 Detén antes de resetear
                countdownTimer.ResetTimer();
                transactionGoal.GenerateNewGoal();
                countdownTimer.StartTimer();

                yield return new WaitForSeconds(30f);

                

                anim.speed = 1f;
                direccion = -1; // Empieza a retroceder
            }
            else if (indice == 0)
            {
                if (!primeraVez) // Si no es la primera vez que llega al punto 0
                {
                    anim.speed = 0f;
                    yield return new WaitForSeconds(5f);
                    anim.speed = 1f;
                }
                else
                {
                    primeraVez = false; // Marca que ya llegó una vez
                }

                direccion = 1; // Empieza a avanzar
            }

            // Cambiar al siguiente punto según la dirección
            indice += direccion;

            // Asegurar que el índice se mantenga dentro de los límites
            indice = Mathf.Clamp(indice, 0, puntos.Length - 1);
        }
    }
}
