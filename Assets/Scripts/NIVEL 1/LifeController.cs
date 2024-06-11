using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeController : MonoBehaviour
{
    public List<HeartView> hearthViews = new List<HeartView>();

    public void UpdateLife(int amount)
    {

        foreach (var hearth in hearthViews)
        {
            hearth.UpdateHeart(0);
        }

        int amountHearth = (int)amount / 2;

        for (int i = 0; i < amountHearth; i++)
        {
            hearthViews[i].UpdateHeart(2);
        }

        int currentHearth = (int)amount % 2;
        hearthViews[amountHearth].UpdateHeart(currentHearth);

    }
}
