using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeartView : MonoBehaviour
{
    public Sprite vidaMaxima;
    public Sprite vidaCinco;
    public Sprite vidaCuatro;
    public Sprite vidaTres;
    public Sprite vidados;
    public Sprite vidauno;
    public Sprite sinVida;
    private Image imageRender;

    private void Awake()
    {
        imageRender = GetComponent<Image>();
    }

    public void UpdateHeart(int lives)
    {
        switch (lives)
        {
            case 6:
                imageRender.sprite = vidaMaxima;
                break;
            case 5:
                imageRender.sprite = vidaCinco;
                break;
            case 4:
                imageRender.sprite = vidaCuatro;
                break;
            case 3:
                imageRender.sprite = vidaMaxima;
                break;
            case 2:
                imageRender.sprite = vidaMaxima;
                break;
            case 1:
                imageRender.sprite = vidaMaxima;
                break;
            case 0:
                imageRender.sprite = vidaMaxima;
                break;

        }
    }
}

