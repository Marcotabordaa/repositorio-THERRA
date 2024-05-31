using UnityEngine;
using System.Collections;

public class PuzzleExplosion: MonoBehaviour
{
    
    public float minForce = 100f;        // Fuerza mínima de dispersión
    public float maxForce = 500f;       // Fuerza máxima de dispersión
    public float forceDuration = 300f; // Duración de la fuerza inicial
    public float moveDuration = 2f;    // Duración del movimiento controlado
    public float moveSpeed = 2f;       // Velocidad de movimiento

    private bool isMoving = false;

    void Start()
    {
        // Obtener el Rigidbody2D del objeto
        Rigidbody2D rb2D = GetComponent<Rigidbody2D>();

        if (rb2D != null)
        {
            // Generar una dirección aleatoria en el plano XY
            Vector2 randomDirection = Random.insideUnitCircle.normalized;
            // Generar una magnitud de fuerza aleatoria entre minForce y maxForce
            float randomForce = Random.Range(minForce, maxForce);

            // Aplicar la fuerza a la pieza del rompecabezas
            rb2D.AddForce(randomDirection * randomForce, ForceMode2D.Impulse);

            // Iniciar la corrutina para detener la fuerza inicial después de cierto tiempo
            StartCoroutine(StopInitialForce(rb2D, forceDuration));
        }
    }

    private IEnumerator StopInitialForce(Rigidbody2D rb2D, float duration)
    {
        yield return new WaitForSeconds(duration);

        // Detener la fuerza inicial
        rb2D.velocity = Vector2.zero;
        rb2D.angularVelocity = 0f;

        // Iniciar el movimiento controlado
        isMoving = true;
        StartCoroutine(MovePiece(rb2D));
    }

    private IEnumerator MovePiece(Rigidbody2D rb2D)
    {
        float elapsedTime = 0f;
        while (elapsedTime < moveDuration)
        {
            // Mover la pieza en una dirección aleatoria
            Vector2 randomDirection = Random.insideUnitCircle.normalized;
            rb2D.velocity = randomDirection * moveSpeed;

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Detener el movimiento controlado después del tiempo especificado
        rb2D.velocity = Vector2.zero;
        rb2D.angularVelocity = 0f;
        isMoving = false;
    }
}