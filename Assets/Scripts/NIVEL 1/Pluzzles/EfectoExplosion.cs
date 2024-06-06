using UnityEngine;
using System.Collections;

public class EfectoExplosion : MonoBehaviour
{
    public float minForce = 30f;        // Fuerza mínima de dispersión
    public float maxForce = 60f;       // Fuerza máxima de dispersión
    public float forceDuration = 0.5f; // Duración de la fuerza inicial
    public float moveDuration = 1f;    // Duración del movimiento controlado
    public float moveSpeed = 2f;       // Velocidad de movimiento
    public new ParticleSystem particleSystem;  // Referencia al sistema de partículas
    private float radioDeExplosión = 100f;
    //private bool isMoving = false;

    void Start()
    {
        // Reproducir el sistema de partículas
        if (particleSystem != null)
        {
            particleSystem.Play();
        }

        // Obtener el Rigidbody del objeto
        Rigidbody rb = GetComponent<Rigidbody>();

        if (rb != null)
        {
            // Generar una dirección aleatoria en el plano XY
            Vector3 randomDirection = Random.insideUnitCircle.normalized;
            // Generar una magnitud de fuerza aleatoria entre minForce y maxForce
            float randomForce = Random.Range(minForce, maxForce);

            // Aplicar la fuerza a la pieza del rompecabezas
            rb.AddForce(randomDirection * randomForce, ForceMode.Impulse);

            // Iniciar la corrutina para detener la fuerza inicial después de cierto tiempo
            StartCoroutine(StopInitialForce(rb, forceDuration));
        }
    }

    private IEnumerator StopInitialForce(Rigidbody rb, float duration)
    {
        yield return new WaitForSeconds(duration);

        // Detener la fuerza inicial
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

       /* // Iniciar el movimiento controlado
        isMoving = true;
        StartCoroutine(MovePiece(rb));*/
    }

    private IEnumerator MovePiece(Rigidbody rb)
    {
        float elapsedTime = 0f;
        while (elapsedTime < moveDuration)
        {
            // Mover la pieza en una dirección aleatoria
            Vector2 randomDirection = Random.insideUnitCircle.normalized;
            rb.velocity = randomDirection * moveSpeed;

            elapsedTime += Time.deltaTime;
            yield return null;
        }

       /* // Detener el movimiento controlado después del tiempo especificado
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        isMoving = false;*/
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radioDeExplosión);
    }
}
