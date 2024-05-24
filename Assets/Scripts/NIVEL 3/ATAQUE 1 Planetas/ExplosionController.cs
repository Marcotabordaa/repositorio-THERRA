using UnityEngine;

public class ExplosionController : MonoBehaviour
{
    public ParticleSystem explosionEffect;
    public AudioSource explosionSound; // Nuevo: Agregamos un AudioSource para el sonido
    public float expansionSpeed = 10f;
    public float maxRadius = 50f;
    private bool isExpanding = false;
    private float currentRadius = 0f;

    void Start()
    {
        // Asegurarse de que el sistema de partículas esté detenido al inicio
        if (explosionEffect.isPlaying)
        {
            explosionEffect.Stop();
        }
    }

    void Update()
    {
        if (isExpanding)
        {
            currentRadius += expansionSpeed * Time.deltaTime;
            var main = explosionEffect.main;
            main.startSpeed = currentRadius;

            if (currentRadius >= maxRadius)
            {
                isExpanding = false;
                currentRadius = 0f;
                explosionEffect.Stop();
            }
        }
    }

    public void TriggerExplosion()
    {
        if (!explosionEffect.isPlaying)
        {
            explosionEffect.Play();
            isExpanding = true;

            if (explosionSound != null) // Nuevo: Verificar si hay un AudioSource asignado
            {
                explosionSound.Play(); // Nuevo: Reproducir el sonido de la explosión
            }
        }
    }
}


