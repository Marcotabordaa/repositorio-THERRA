using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameTimer : MonoBehaviour
{
    public PuzzleChecker checker;
    public float timeRemaining = 300;
    public TextMeshProUGUI timerText;
    private bool timerIsRunning;

    public void Initialize()
    {
        timerIsRunning = true;
    }

    void Update()
    {
        UpdateTimerUI();
        
        if (timerIsRunning)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
            }
            else
            {
                timeRemaining = 0;
                timerIsRunning = false;

                checker.Die();
            }
        }
    }

    public void ReduceTime(float amount)
    {
        timeRemaining -= amount;
        if (timeRemaining < 0) timeRemaining = 0;
    }

    public void AddTime(float amount)
    {
        timeRemaining += amount;
    }

    private void UpdateTimerUI()
    {
        timerText.text = FormatTime(timeRemaining);
    }

    private string FormatTime(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60);
        int seconds = Mathf.FloorToInt(time % 60);
        return string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}