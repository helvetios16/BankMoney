using UnityEngine;
using UnityEngine.UI;
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
    public BlinkOnBool blinkOnBool;

    private bool _shouldInterrupt = false;
    private Coroutine moverCoroutine;

    [Header("Audio de disparo y alarma")]
    public AudioClip disparoClip;
    public AudioClip alarmaClip;
    [Range(0f, 1f)]
    public float probabilidadDisparo = 0.35f;
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

    void Start()
    {
        anim = GetComponent<Animator>();
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;

        if (pistolObject != null)
            pistolObject.SetActive(false);

        if (objetoActivar != null)
            objetoActivar.SetActive(false);

        if (cono1 != null) cono1.SetActive(false);
        if (cono2 != null) cono2.SetActive(false);

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

            if (indice == puntos.Length - 1)
            {
                anim.speed = 0f;

                countdownTimer.StopTimer();
                countdownTimer.ResetTimer();
                transactionGoal.GenerateNewGoal();
                countdownTimer.StartTimer();

                blinkOnBool.StartBlink();

                _shouldInterrupt = false;

                float waitTime = 30f;
                float elapsed = 0f;

                bool disparoDecidido = Random.value <= probabilidadDisparo;
                bool disparoHecho = false;

                while (elapsed < waitTime)
                {
                    elapsed += Time.deltaTime;

                    if (_shouldInterrupt) break;

                    if (disparoDecidido && !disparoHecho)
                    {
                        alarmaCoroutine = StartCoroutine(DisparoYAlarma());
                        disparoHecho = true;
                    }

                    yield return null;
                }

                blinkOnBool.StopBlink();

                anim.speed = 1f;
                direccion = -1;
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
            _shouldInterrupt = false;
        }
    }

    IEnumerator DisparoYAlarma()
    {
        // Mostrar pistola
        if (pistolObject != null)
            pistolObject.SetActive(true);

        // Mostrar conos
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
        if (pistolObject != null)
            pistolObject.SetActive(false);
        if (cono1 != null) cono1.SetActive(false);
        if (cono2 != null) cono2.SetActive(false);

        alarmaActiva = false;
    }

    public void InterruptAndGoBack()
    {
        _shouldInterrupt = true;
    }

    // 🔴 Método simple: botón como parámetro
    public void ApagarAlarma(Button boton)
    {
        if (boton == null) return;

        // Cambiar color del Image del botón a rojo
        Image img = boton.GetComponent<Image>();
        if (img != null)
            img.color = Color.red;

        // Apagar la alarma y el disparo
        if (alarmaActiva)
        {
            if (alarmaCoroutine != null)
                StopCoroutine(alarmaCoroutine);

            if (audioSource.isPlaying)
                audioSource.Stop();

            if (objetoActivar != null)
                objetoActivar.SetActive(false);

            if (cono1 != null) cono1.SetActive(false);
            if (cono2 != null) cono2.SetActive(false);

            if (pistolObject != null)
                pistolObject.SetActive(false);

            alarmaActiva = false;
        }
    }
}
