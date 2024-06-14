using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LifesView : MonoBehaviour
{
    public List<Image> lifeImages;

    public void UpdateHeart(int lives)
    {
        foreach(var image in lifeImages)
        {
            image.gameObject.SetActive(false);
        }

        for(int i = 0; i < lives; i++)
        {
            lifeImages[i].gameObject.SetActive(true);
        }
    }
}

