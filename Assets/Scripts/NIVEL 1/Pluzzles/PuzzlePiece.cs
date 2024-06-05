using UnityEngine;

public class PuzzlePiece : MonoBehaviour
{
    private bool isHeld = false; // Indica si la pieza est? siendo sostenida por el jugador
    private Vector3 originalPosition; // Posici?n original de la pieza
    public bool isCorrectlyPlaced = false; // Indica si la pieza est? correctamente colocada
    public Transform correctPosition; // La posici?n correcta que debe alcanzar la pieza
    private Transform playerHand; // Referencia al transform de la mano del jugador
    private Rigidbody rb; // Referencia al Rigidbody de la pieza

    void Start()
    {
        originalPosition = transform.position;
        playerHand = GameObject.Find("PlayerHand").transform; // Busca el GameObject de la mano del jugador
        rb = GetComponent<Rigidbody>();
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
            rb.isKinematic = true; // Desactiva la f?sica mientras se sostiene la pieza
            transform.position = playerHand.position; // Coloca la pieza en la posici?n de la mano del jugador
            transform.SetParent(playerHand); // Adjunta la pieza a la mano del jugador

        }
    }

    void DropPiece()
    {
        if (isHeld)
        {
            isHeld = false;
            transform.SetParent(null); // Desadjunta la pieza de la mano del jugador
            rb.useGravity = true; // Reactiva la gravedad cuando se suelta la pieza
            rb.isKinematic = false; // Reactiva la f?sica cuando se suelta la pieza
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
        }
        else
        {
            transform.position = originalPosition; // Vuelve a la posici?n original si no est? correctamente colocada
            rb.isKinematic = false; // Reactiva la f?sica
        }
    }
}