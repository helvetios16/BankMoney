using UnityEngine;

public class GameEndController : MonoBehaviour
{
    public GameObject ganastePanel;
    public GameObject perdistePanel;

    void Start()
    {
        // Asegúrate que ambos paneles estén ocultos al inicio
        if (ganastePanel != null) ganastePanel.SetActive(false);
        if (perdistePanel != null) perdistePanel.SetActive(false);
    }

    public void ShowWin()
    {
        Debug.Log("Activando panel de Ganar");
        if (ganastePanel != null) ganastePanel.SetActive(true);
        Debug.Log("ActiveSelf: " + ganastePanel.activeSelf + ", ActiveInHierarchy: " + ganastePanel.activeInHierarchy);
        PauseGame();
    }

    public void ShowLose()
    {
        if (perdistePanel != null) perdistePanel.SetActive(true);
        PauseGame();
    }

    private void PauseGame()
    {
        Time.timeScale = 0f;  // pausar toda la simulación de físicas, animaciones, etc
        // Opcional: desactivar interacciones XR
        // por ejemplo: XRInteractionManager.enabled = false;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
        if (ganastePanel != null) ganastePanel.SetActive(false);
        if (perdistePanel != null) perdistePanel.SetActive(false);
        // Opcional: reactivar interacciones XR
    }
}