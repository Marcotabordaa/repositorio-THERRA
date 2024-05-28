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
            if (!piece.GetComponent<PuzzlePiece>().isCorrectlyPlaced)
            {
                return false;
            }
        }
        return true;
    }

    void UnlockPower()
    {
        Debug.Log("Puzzle completo! Poder desbloqueado!");
        // Aquí puedes añadir la lógica para desbloquear el poder
    }
} 

