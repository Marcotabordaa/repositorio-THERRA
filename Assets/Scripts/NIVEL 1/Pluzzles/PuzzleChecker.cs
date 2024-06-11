using UnityEngine;

public class PuzzleChecker : MonoBehaviour
{
    public GameObject[] puzzlePieces; // Asigna las piezas del puzzle en el inspector
    private bool isPuzzleComplete = false;
    private int piezas = 0;
    bool abierto = false;
    public OpenDoor openDoor;

    void Update()
    {
        if (!isPuzzleComplete && CheckPuzzleComplete())
        {
            isPuzzleComplete = true;
            LiberarPuerta();
        }
    }

    bool CheckPuzzleComplete()
    {
        piezas = 0; // Resetear el contador de piezas correctamente colocadas
        foreach (GameObject piece in puzzlePieces)
        {
            PuzzlePiece puzzlePiece3D = piece.GetComponent<PuzzlePiece>();
            if (puzzlePiece3D == null || !puzzlePiece3D.isCorrectlyPlaced)
            {
                return false;
            }
            piezas++;
        }
        return true;
    }

    void LiberarPuerta()
         
    {
        Debug.Log("N�mero de piezas correctamente colocadas: " + piezas);
        if (piezas >= puzzlePieces.Length) // Aseg�rate de comparar con el n�mero total de piezas
        {
            Debug.Log("Todas las piezas est�n en su lugar. Abriendo la puerta.");
            abierto = true;
            openDoor.Open(abierto);
        }
    }
}
