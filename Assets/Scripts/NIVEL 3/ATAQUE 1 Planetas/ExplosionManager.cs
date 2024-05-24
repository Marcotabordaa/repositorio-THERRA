using UnityEngine;

public class ExplosionManager : MonoBehaviour
{
    public ExplosionController[] explosionControllers; // Array de controladores de explosión
    public Renderer centralPlanetRenderer; // Referencia al Renderer del planeta central
    private int redShots = 0;
    private int blueShots = 0;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1) && explosionControllers.Length > 0)
        {
            explosionControllers[0].TriggerExplosion();
            UpdatePlanetColor(true); // Disparo rojo
        }
        if (Input.GetKeyDown(KeyCode.Alpha2) && explosionControllers.Length > 1)
        {
            explosionControllers[1].TriggerExplosion();
            UpdatePlanetColor(false); // Disparo azul
        }
    }

    void UpdatePlanetColor(bool isRedShot)
    {
        if (isRedShot)
        {
            if (blueShots > 0)
            {
                blueShots--;
            }
            else
            {
                redShots++;
            }
        }
        else
        {
            if (redShots > 0)
            {
                redShots--;
            }
            else
            {
                blueShots++;
            }
        }

        // Limitar el número de disparos totales a 5
        int totalShots = redShots + blueShots;
        if (totalShots > 5)
        {
            redShots = Mathf.Min(redShots, 5 - blueShots);
            blueShots = Mathf.Min(blueShots, 5 - redShots);
        }

        // Calcular el nuevo color basado en los disparos
        float redIntensity = (float)blueShots / 5f; // Invertido para que 1 haga rojo
        float blueIntensity = (float)redShots / 5f; // Invertido para que 2 haga azul
        float greenIntensity = 0f; // Mantener el verde en 0 para simplificar la mezcla

        Color newColor = new Color(redIntensity, greenIntensity, blueIntensity);
        centralPlanetRenderer.material.color = newColor;
    }
}
