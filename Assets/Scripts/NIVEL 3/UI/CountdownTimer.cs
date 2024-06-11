using UnityEngine;
using TMPro;

public class CountdownTimer : MonoBehaviour
{
    public float tiempoInicial = 60f;
    private float tiempoActual;
    public TMP_Text textoTiempo;
    public GameObject panelMensaje;

    void Start()
    {
        tiempoActual = tiempoInicial;
        panelMensaje.SetActive(false); // Desactivar el panel al inicio
    }

    void Update()
    {
        tiempoActual -= Time.deltaTime;

        if (tiempoActual <= 0)
        {
            tiempoActual = 0;
            HandleTimerEnd();
        }

        textoTiempo.text = "CRONOS: " + Mathf.RoundToInt(tiempoActual) + " S";
    }

    void HandleTimerEnd()
    {
        textoTiempo.gameObject.SetActive(false); // Desactivar el texto del temporizador
        panelMensaje.SetActive(true); // Activar el panel del mensaje final
        Debug.Log("¡Tiempo terminado!");
    }
}
