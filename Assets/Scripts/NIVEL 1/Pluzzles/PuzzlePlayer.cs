using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzlePlayer : MonoBehaviour
{
    public Camera camara; // Referencia a la c�mara
    public Transform hand; // Referencia a la mano
    public float distanciaRecoger; // Distancia a la que el jugador puede recoger piezas
    
    private PuzzlePiece piezaSostenida = null; // La pieza que el jugador est� sosteniendo

    void Update()
    {
        // Verifica si el jugador hace clic
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("Mouse clicked");
            // Si no est� sosteniendo una pieza, intenta recoger una
            if (piezaSostenida == null)
            {
                RecogerPieza();
            }
            // Si est� sosteniendo una pieza, la suelta
            else
            {
                SoltarPieza();
            }
        }
    }

    void RecogerPieza()
    {
        Ray ray = camara.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        Debug.Log("Casting ray from: " + ray.origin + " in direction: " + ray.direction);

        if (Physics.Raycast(ray, out hit, distanciaRecoger))
        {
            Debug.Log("Raycast hit: " + hit.transform.name + " at distance: " + hit.distance);
            {
                Debug.Log("Raycast hit: " + hit.transform.name);
                Debug.Break(); // Esto pausar� el juego en este punto
                               // Resto del c�digo...
            }
        }
        else
        {
            Debug.Log("Raycast did not hit anything within " + distanciaRecoger + " units");
        }
        Debug.DrawRay(ray.origin, ray.direction * distanciaRecoger, Color.red, 1f);
        
        // Lanza un rayo desde la posici�n de la c�mara en la direcci�n en la que est� apuntando
        if (Physics.Raycast(camara.ScreenPointToRay(Input.mousePosition), out hit, distanciaRecoger))
        {
            Debug.Log("Raycast hit: " + hit.transform.name);
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
        // Suelta la pieza y la deja en la posici�n actual del cursor
        piezaSostenida.DropPiece();
        piezaSostenida = null;
    }
    private void OnDrawGizmos()
    {
        if (camara != null)
        {
            Gizmos.color = Color.yellow;
            Vector3 direction = camara.transform.forward * distanciaRecoger;
            Gizmos.DrawRay(camara.transform.position, direction);
        }
    }
}
