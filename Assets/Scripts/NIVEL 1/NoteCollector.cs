using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteCollector : MonoBehaviour
{
    public GameTimer gameTimer;  
    public float timeToAdd = 30f;

    private int score = 0;

        void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Note"))
            {
                score++;
                Debug.Log("Score: " + score);
                Destroy(other.gameObject);
            }

        if (gameTimer != null)
        {
            gameTimer.AddTime(timeToAdd);
        }

    }

}

