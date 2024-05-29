using UnityEngine;

public class Personaje : MonoBehaviour
{
    public float velRotacion = 90f;
    public float moveSpeed = 10f; // Velocidad de movimiento del jugador
    public float jumpForce = 5f; // Fuerza de salto del jugador (ajustada)
    private Rigidbody rb; // Referencia al Rigidbody del jugador
    private bool isGrounded;

    public static Personaje singleton;

    private void Awake()
    {
        singleton = this;
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // Movimiento horizontal y vertical
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        // Calcular el movimiento en funci?n de las entradas horizontales y verticales
        Vector3 movement = new Vector3(horizontalInput, 0f, verticalInput) * moveSpeed * Time.deltaTime;

        // Rotar el movimiento de acuerdo con la rotaci?n del objeto
        movement = Quaternion.Euler(0, transform.eulerAngles.y, 0) * movement;

        // Aplicar el movimiento al objeto
        transform.Translate(movement, Space.World);

        // Verificar si el jugador est? en el suelo
        isGrounded = Physics.Raycast(transform.position, Vector3.down, 1.1f); // Ajusta la distancia seg?n sea necesario

        // Saltar si el jugador est? en el suelo y se presiona la tecla de salto (por ejemplo, barra espaciadora)
        if (Input.GetButtonDown("Jump") && isGrounded) // Aseg?rate de que el nombre del bot?n es "Jump"
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    private void OnDrawGizmos()
    {
        // Dibujar el raycast en la escena para depuraci?n
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, Vector3.down * 1.1f); // Ajusta la distancia seg?n sea necesario
    }
}

