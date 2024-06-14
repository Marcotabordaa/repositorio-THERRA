using UnityEngine;

public class PuzzlePiece : MonoBehaviour
{
    public bool isCorrectlyPlaced = false;
    public Transform correctPosition;
    private Rigidbody rb;
    private Vector3 originalPosition;
    public GameObject ligth;

    void Start()
    {
        originalPosition = transform.position;
        rb = GetComponent<Rigidbody>(); 
    }

    public void PickUpPiece(Transform playerHand)
    {
        rb.useGravity = false; // Desactiva la gravedad mientras se sostiene la pieza
        rb.isKinematic = true; // Desactiva la física mientras se sostiene la pieza
        transform.position = playerHand.position; // Coloca la pieza en la posición de la mano del jugador
        transform.SetParent(playerHand);// Adjunta la pieza a la mano del jugador
        ligth.GetComponent<Light>().enabled = false;
    }

    public void DropPiece()
    {
        transform.SetParent(null); // Desadjunta la pieza de la mano del jugador
        rb.useGravity = true; // Reactiva la gravedad cuando se suelta la pieza
        rb.isKinematic = false; // Reactiva la física cuando se suelta la pieza
        ligth.GetComponent <Light>().enabled = true;
        CheckPlacement();
    }

    public void CheckPlacement()
    {
        // En lugar de verificar la distancia aqui, lo podemos hacer desde el otro lugar "PuzzleChecker".
        // para eso, lo podemos hacer con un SphereCast
        
        // PENDIENTE
        float distance = Vector3.Distance(transform.position, correctPosition.position);
        if (distance < 8f) // Tolerancia para considerar la pieza correctamente colocada
        {
            transform.position = correctPosition.position;
            isCorrectlyPlaced = true;
            rb.isKinematic = true; // Fija la pieza en su lugar una vez colocada correctamente
            Debug.Log("Pieza correctamente colocada");
        }
        else
        {
            rb.isKinematic = false; // Reactiva la física
            Debug.Log("Pieza no colocada correctamente");
        }
    }
}
