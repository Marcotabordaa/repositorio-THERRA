using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteCollector : MonoBehaviour
{
    public int score;
    public GameTimer gameTimer;
    public float timeToAdd = 30f;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Note"))
        {
            Destroy(other.gameObject);
            gameTimer.AddTime(timeToAdd);
            score++;
        }
    }

}

