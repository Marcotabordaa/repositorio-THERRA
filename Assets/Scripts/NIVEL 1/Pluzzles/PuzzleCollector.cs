using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PuzzleCollector : MonoBehaviour
{
    public TMP_Text piecesText;
    public List<PuzzlePiece> puzzlePieces;
    public PuzzleChecker checker;

    private int totalPieces;
    private int currentAmountPieces;

    private void Awake()
    {
        puzzlePieces = new List<PuzzlePiece>();
    }

    private void Start()
    {
        totalPieces = checker.puzzlePieces.Count;
    }

    private void Update()
    {
        piecesText.text = $"Pieces: {currentAmountPieces}/{totalPieces}";
    }

    public void Collect(PuzzlePiece piece)
    {
        puzzlePieces.Add(piece);
        currentAmountPieces++;
        CheckPuzzlePieces();
    }

    public void CheckPuzzlePieces()
    {
        if (puzzlePieces.Count == checker.puzzlePieces.Count)
        {
            checker.PuzzleCompleted();
        }
    }
}
