using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Died : MonoBehaviour
{
    public Image[] heart;
    public int life = 6;
    private int _heart = 0;
    //public MovimientoJugador movePlayer;
    //private Animator anim;
    void Start()
    {
        //anim = GetComponent<Animator>();
        //movePlayer = GetComponent<MovimientoJugador>();
    }

}