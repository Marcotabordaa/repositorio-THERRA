using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzlePlayer : MonoBehaviour
{
    public Camera camara; // Referencia a la cámara
    public Transform hand; // Referencia a la mano
    public float distanciaRecoger; // Distancia a la que el jugador puede recoger piezas
    
    private PuzzlePiece piezaSostenida = null; // La pieza que el jugador está sosteniendo

    void Update()
    {
        // Verifica si el jugador hace clic
        if (Input.GetMouseButtonDown(0))
        {
            // Si no está sosteniendo una pieza, intenta recoger una
            if (piezaSostenida == null)
            {
                RecogerPieza();
            }
            // Si está sosteniendo una pieza, la suelta
            else
            {
                SoltarPieza();
            }
        }
    }

    void RecogerPieza()
    {
        RaycastHit hit;
        // Lanza un rayo desde la posición de la cámara en la dirección en la que está apuntando
        if (Physics.Raycast(camara.ScreenPointToRay(Input.mousePosition), out hit, distanciaRecoger))
        {
            // Si el rayo golpea una pieza de puzzle, la recoge
            PuzzlePiece pieza = hit.transform.GetComponent<PuzzlePiece>();
            if (pieza != null && !pieza.isCorrectlyPlaced)
            {
                piezaSostenida = pieza;
                // Ajusta la pieza para que siga al cursor
                piezaSostenida.PickUpPiece(hand);
            }
        }
    }

    void SoltarPieza()
    {
        // Suelta la pieza y la deja en la posición actual del cursor
        piezaSostenida.DropPiece();
        piezaSostenida = null;
    }
}
