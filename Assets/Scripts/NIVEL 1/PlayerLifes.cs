using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLifes: MonoBehaviour
{
    public LifesView lifesView;
    public int lifes;
    public PuzzleChecker checker;

    private void Update()
    {
        lifesView.UpdateHeart(lifes);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Water"))
        {
            lifes--;
        }

        if (lifes <= 0)
        {
            checker.Die();
        }
    }

}
