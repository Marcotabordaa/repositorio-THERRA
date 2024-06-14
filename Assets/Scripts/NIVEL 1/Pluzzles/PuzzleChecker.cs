using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleChecker : MonoBehaviour
{
    public List<PuzzlePiece> puzzlePieces; // Asigna las piezas del puzzle en el inspector
    public GameTimer gameTimer;
   // public TimelineController timelineController;
    private void Start()
    {
        // TODO: Activar timeline inicio de juego

        //timelineController.PlayStartTimeline();
        StartCoroutine(StartGame());
        Debug.Log("Inicio del juego");
        StartCoroutine(StartGame());
    }

    private IEnumerator StartGame()
    {
        Debug.Log("Inicia animacion de explosion");
        yield return new WaitForSeconds(10);
        // Inicia el timer del juego.
        gameTimer.Initialize();
    }

    public void PuzzleCompleted()
    {
        // TODO: Activar el timeline de fin de juego
        Debug.Log("Completado");
        //timelineController.PlayEndTimeline();
       
        StartCoroutine(CompletedGame());
    }

    private IEnumerator CompletedGame()
    {
        Debug.Log("Completaste el juego");
        yield return new WaitForSeconds(10);
        Debug.Log("Cambio a escena nivel 2");
        Debug.Log("Fin");
    }

    public void Die()
    {
        // TODO: Activar el timeline de perder
      
        
       // timelineController.PlayLoseTimeline();
        StartCoroutine(DieGame());
        Debug.Log("MORISTE");
        
    }

    private IEnumerator DieGame()
        
    {   
        //timelineController.PlayLoseTimeline();

        Debug.Log("Animacion Perdiste");
        // lo hace

        yield return new WaitForSeconds(5);
        // Cambio de escena a menu
        Debug.Log("Fin");

        yield return new WaitForSeconds(5);
        // Cambio de escena a menu
        Debug.Log("Fin");
    }
}
