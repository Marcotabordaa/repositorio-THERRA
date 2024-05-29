using UnityEngine;

public class PuzzleChecker : MonoBehaviour
{
    public GameObject[] puzzlePieces; // Asigna las piezas del puzzle en el inspector
    private bool isPuzzleComplete = false;

    void Update()
    {
        if (!isPuzzleComplete && CheckPuzzleComplete())
        {
            isPuzzleComplete = true;
            UnlockPower();
        }
    }

    bool CheckPuzzleComplete()
    {
        foreach (GameObject piece in puzzlePieces)
        {
            PuzzlePiece puzzlePiece3D = piece.GetComponent<PuzzlePiece>();
            if (puzzlePiece3D == null || !puzzlePiece3D.isCorrectlyPlaced)
            {
                return false;
            }
        }
        return true;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PuzzlePiece"))
        {
            Debug.Log("Pieza del puzzle detectada en el ?rea de descarga.");
            other.GetComponent<PuzzlePiece>().CheckPlacement();
        }
    }

    void UnlockPower()
    {
        Debug.Log("Puzzle completo! Poder desbloqueado!");
        // Aqu? puedes a?adir la l?gica para desbloquear el poder
    }
}
