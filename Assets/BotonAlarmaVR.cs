using UnityEngine;
using UnityEngine.UI;

public class BotonAlarmaVR : MonoBehaviour
{
    public NPCPathFollower npcPathFollower; // Arrastra tu NPC aquí desde la escena
    private Button boton;
    private Image botonImage;

    void Start()
    {
        boton = GetComponent<Button>();
        botonImage = GetComponent<Image>();

        if (boton != null)
            boton.onClick.AddListener(OnButtonPressed);
    }

    void OnButtonPressed()
    {
        // Cambiar color del botón a rojo
        if (botonImage != null)
            botonImage.color = Color.red;

        // Apagar alarma, pistola y conos
        if (npcPathFollower != null)
        {
            npcPathFollower.ApagarAlarma(boton);
        }
    }
}