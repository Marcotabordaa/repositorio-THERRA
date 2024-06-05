using UnityEngine;

public class PuzzleChecker : MonoBehaviour
{
    public GameObject[] puzzlePieces; // Asigna las piezas del puzzle en el inspector
    private bool isPuzzleComplete = false;
    private int piezas;
    bool abierto = false;
    public OpenDoor openDoor;

    public PuzzleChecker(bool _abierto)
    {
        abierto = _abierto;
    }

    void Update()
    {
        if (!isPuzzleComplete && CheckPuzzleComplete() )
        {
            isPuzzleComplete = true;
            LiberarPuerta();
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
           piezas++;
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

    void LiberarPuerta()
    {
        
        if(piezas >= 23)
        {
            abierto = true;
            openDoor.Open(abierto);
        }
    }


}
