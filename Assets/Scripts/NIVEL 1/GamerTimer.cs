using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameTimer : MonoBehaviour
{
    public float timeRemaining = 300;
    public TextMeshProUGUI timerText;
    private bool timerIsRunning = true;

    void Start()
    {
        UpdateTimerUI();
    }

    void Update()
    {
        if (timerIsRunning)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
                UpdateTimerUI();
            }
            else
            {
                timeRemaining = 0;
                timerIsRunning = false;
                // Lógica para cuando el tiempo se agote
                Debug.Log("Time has run out!");
                SceneManager.LoadScene("GameOverScene");
            }
        }
    }

    public void ReduceTime(float amount)
    {
        timeRemaining -= amount;
        if (timeRemaining < 0) timeRemaining = 0;
        UpdateTimerUI();
    }

    public void AddTime(float amount)
    {
        timeRemaining += amount;
        UpdateTimerUI();
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