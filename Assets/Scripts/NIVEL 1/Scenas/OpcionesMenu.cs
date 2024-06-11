using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine. SceneManagement;

public class OpcionesMenu : MonoBehaviour
{
    
     public void IniciarJuego (string NivelTherra)
    { 
        SceneManager.LoadScene(NivelTherra);
    }

    public void Salir()
    {
         Application.Quit();
    }

}
