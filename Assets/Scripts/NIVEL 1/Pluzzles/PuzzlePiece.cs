using UnityEngine;

using UnityEngine;

public class PuzzlePiece : MonoBehaviour
{
    private bool isHeld = false; // toma la pieza
    private Vector3 originalPosition;
    public bool isCorrectlyPlaced = false; // Indica si la pieza está correctamente colocada
    public Transform correctPosition; // La posición correcta que debe alcanzar la pieza
    private Transform playerHand; // Referencia al transform de la mano del jugador
    private Rigidbody rb; // Referencia al Rigidbody de la pieza

    void Start()
    {
        originalPosition = transform.position;
        playerHand = GameObject.Find("PlayerHand").transform; // Busca el GameObject de la mano del jugador
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (isHeld)
        {
            transform.position = playerHand.position; // Actualiza la posición continuamente mientras se sostiene la pieza
        }
    }

    void OnMouseDown()
    {
        if (!isHeld)
        {
            PickUpPiece();
        }
        else
        {
            DropPiece();
        }
    }

    void PickUpPiece()
    {
        if (!isCorrectlyPlaced)
        {
            isHeld = true;
            rb.useGravity = false; // Desactiva la gravedad mientras se sostiene la pieza
            rb.isKinematic = true; // Desactiva la física mientras se sostiene la pieza
        }
    }

    void DropPiece()
    {
        if (isHeld)
        {
            isHeld = false;
            rb.useGravity = true; // Reactiva la gravedad cuando se suelta la pieza
            rb.isKinematic = false; // Reactiva la física cuando se suelta la pieza
            CheckPlacement();
        }
    }

    public void CheckPlacement()
    {
        float distance = Vector3.Distance(transform.position, correctPosition.position);
        if (distance < 0.5f) // Tolerancia para considerar la pieza correctamente colocada
        {
            transform.position = correctPosition.position;
            isCorrectlyPlaced = true;
            rb.isKinematic = true; // Fija la pieza en su lugar una vez colocada correctamente
            rb.useGravity = false; // Aseguramos que no afecte la gravedad
        }
        else
        {
            transform.position = originalPosition; // Vuelve a la posición original si no está correctamente colocada
            rb.isKinematic = false; // Reactiva la física
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.transform == correctPosition && !isHeld)
        {
            transform.position = correctPosition.position;
            isCorrectlyPlaced = true;
            rb.isKinematic = true; // Fija la pieza en su lugar una vez colocada correctamente
            rb.useGravity = false; // Aseguramos que no afecte la gravedad
        }
    }
}