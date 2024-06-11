using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    private int playerScore = 0;
    private int redScore = 0;
    public int scoreLimit = 90; // Límite de puntuación
    public TextMeshProUGUI playerScoreText;
    public TextMeshProUGUI redScoreText;

    void Start()
    {
        UpdatePlayerScoreText();
        UpdateRedScoreText();
    }

    public void AddPlayerPoints(int points)
    {
        playerScore += points;
        UpdatePlayerScoreText();
    }

    public void AddRedPoints(int points)
    {
        redScore += points;
        UpdateRedScoreText();
    }

    private void UpdatePlayerScoreText()
    {
        playerScoreText.text = "Hz: " + playerScore;
    }

    private void UpdateRedScoreText()
    {
        redScoreText.text = "Astaroth " + redScore;
    }

    // Método para verificar si se ha alcanzado el límite de puntuación
    public bool IsScoreLimitReached()
    {
        return playerScore >= scoreLimit || redScore >= scoreLimit;
    }

    // Método para verificar si el jugador ha ganado
    public bool IsPlayerWinner()
    {
        return playerScore >= scoreLimit;
    }
}

