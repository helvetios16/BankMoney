using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class NPCPathFollower : MonoBehaviour
{
    [Header("Puntos y movimiento")]
    public Transform[] puntos;
    public float velocidad = 0.5f;
    private int indice = 0;
    private int direccion = 1;
    private Animator anim;
    private bool primeraVez = true;

    [Header("Temporizador y objetivos")]
    public CountdownTimer countdownTimer;
    public TransactionGoalController transactionGoal;
    public BlinkOnBool blinkOnBool;

    private bool _shouldInterrupt = false;
    private Coroutine moverCoroutine;

    [Header("Audio de disparo y alarma")]
    public AudioClip disparoClip;
    public AudioClip alarmaClip;
    [Range(0f, 1f)]
    public float probabilidadDisparo = 0.35f;

    [Header("Audio de diálogo")]
    public AudioClip dialogo1; // Audio que se reproducirá al llegar al último punto
    public AudioClip dialogo2; // Audio que se reproducirá cuando Hungry se active
    private AudioSource audioSource;

    [Header("Pistola (solo visibilidad)")]
    public GameObject pistolObject;

    [Header("Objeto que se activa tras la alarma")]
    public GameObject objetoActivar;

    [Header("Conos a mostrar durante alarma/disparo")]
    public GameObject cono1;
    public GameObject cono2;

    private Coroutine alarmaCoroutine;
    private bool alarmaActiva = false;
    private bool talkingDone = false; // Flag para animación Talking
    private bool dialogoReproducido = false; // Flag para reproducir dialogo1 solo una vez por ciclo

	public ButtonAlertEffect botonAlerta; // arrastra el botón desde el inspector


    void Start()
    {
        anim = GetComponent<Animator>();
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;

        if (pistolObject != null) pistolObject.SetActive(false);
        if (objetoActivar != null) objetoActivar.SetActive(false);
        if (cono1 != null) cono1.SetActive(false);
        if (cono2 != null) cono2.SetActive(false);

        moverCoroutine = StartCoroutine(MoverNPC());
    }

    IEnumerator MoverNPC()
    {
        while (true)
        {
            Vector3 destino = puntos[indice].position;

            // Mover NPC hacia el punto actual
            while (Vector3.Distance(transform.position, destino) > 0.1f)
            {
                transform.position = Vector3.MoveTowards(transform.position, destino, velocidad * Time.deltaTime);
                transform.LookAt(destino);
                yield return null;
            }

            // 🔹 Llegó al último punto
            if (indice == puntos.Length - 1 && !talkingDone)
            {
                talkingDone = true;
                dialogoReproducido = false;

                anim.SetBool("isTalking", true);

                if (dialogo1 != null && !dialogoReproducido)
                {
                    audioSource.PlayOneShot(dialogo1);
                    dialogoReproducido = true;
                }

                countdownTimer.StopTimer();
                countdownTimer.ResetTimer();
                transactionGoal.GenerateNewGoal();
                countdownTimer.StartTimer();
                blinkOnBool.StartBlink();

                bool disparoDecidido = Random.value <= probabilidadDisparo;
                if (disparoDecidido)
                {
                    alarmaCoroutine = StartCoroutine(DisparoYAlarma());
					if (botonAlerta != null)
        				botonAlerta.StartAlert();
                }

                // Espera de 30 segundos con Hungry al segundo 10
                float waitTime = 30f;
                float elapsed = 0f;
                bool hungryActivado = false;
                bool dialogo2Reproducido = false;

                while (elapsed < waitTime)
                {
                    elapsed += Time.deltaTime;

                    // Activar Hungry al segundo 10
                    if (elapsed >= 10f && !hungryActivado)
                    {
                        anim.SetBool("isHungry", true);
                        hungryActivado = true;

                        // Reproducir dialogo2 solo una vez
                        if (dialogo2 != null && !dialogo2Reproducido)
                        {
                            audioSource.PlayOneShot(dialogo2);
                            dialogo2Reproducido = true;
                        }
                    }

                    if (_shouldInterrupt) break;
                    yield return null;
                }

                // Desactivar animaciones y volver a caminar
                anim.SetBool("isTalking", false);
                anim.SetBool("isHungry", false); // esto permite que vuelva a Walking
                blinkOnBool.StopBlink();
                direccion = -1; // volver al primer punto
            }
            // 🔹 Llegó al primer punto
            else if (indice == 0)
            {
                talkingDone = false;
                dialogoReproducido = false;
                if (!primeraVez)
                    yield return new WaitForSeconds(2f);
                else
                    primeraVez = false;

                direccion = 1;
            }

            // Actualizar índice
            indice += direccion;
            indice = Mathf.Clamp(indice, 0, puntos.Length - 1);
            _shouldInterrupt = false;
        }
    }


    IEnumerator DisparoYAlarma()
    {
        // Mostrar pistola y conos
        if (pistolObject != null) pistolObject.SetActive(true);
        if (cono1 != null) cono1.SetActive(true);
        if (cono2 != null) cono2.SetActive(true);

        // Disparo
        if (disparoClip != null)
            audioSource.PlayOneShot(disparoClip);

        yield return new WaitForSeconds(1f);

        // Alarma
        if (alarmaClip != null)
        {
            audioSource.PlayOneShot(alarmaClip);
            alarmaActiva = true;
        }

        yield return new WaitForSeconds(5f);

        // Activar objeto extra si hay
        if (objetoActivar != null)
            objetoActivar.SetActive(true);

        yield return new WaitForSeconds(3f);

        // Desactivar pistola y conos
        if (pistolObject != null) pistolObject.SetActive(false);
        if (cono1 != null) cono1.SetActive(false);
        if (cono2 != null) cono2.SetActive(false);

        alarmaActiva = false;
    }

    public void InterruptAndGoBack()
    {
        _shouldInterrupt = true;
    }

    public void ApagarAlarma(Button boton)
    {
        if (boton == null) return;

        Image img = boton.GetComponent<Image>();
        if (img != null) img.color = Color.red;

        if (alarmaActiva)
        {
            if (alarmaCoroutine != null) StopCoroutine(alarmaCoroutine);
            if (audioSource.isPlaying) audioSource.Stop();
            if (objetoActivar != null) objetoActivar.SetActive(false);
            if (cono1 != null) cono1.SetActive(false);
            if (cono2 != null) cono2.SetActive(false);
            if (pistolObject != null) pistolObject.SetActive(false);
			botonAlerta.StopAlert();
            alarmaActiva = false;
        }
    }
}
