using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class IAEnemigo : MonoBehaviour
{
    public Estado estado;
    public float DistanciaSeguir;
    public float DistanciaAtacar;
    public float DistanciaEscapar;

    private NavMeshAgent agente;

    private void Start()
    {
        agente = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        switch (estado)
        {
            case Estado.idle:
                EstadoIdle();
                break;
            case Estado.siguiendo:
                EstadoSeguir();
                break;
            case Estado.atacando:
                EstadoAtacar();
                break;
            case Estado.muerto:
                EstadoMuerto();
                break;
            default:
                break;
        }


    }

    void EstadoIdle()
    {

    }
    void EstadoSeguir()
    {
        agente.SetDestination(Personaje.singleton.transform.position);

    }
    void EstadoAtacar()
    {

    }
    void EstadoMuerto()
    {

    }
    public void CambiarEstado(Estado NuevoEstado)
    {
        estado = NuevoEstado;
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position,DistanciaSeguir);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, DistanciaAtacar);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, DistanciaEscapar);
    }
}

public enum Estado
{

    idle =      0,
    siguiendo = 1,
    atacando =  2,
    muerto =    3

}