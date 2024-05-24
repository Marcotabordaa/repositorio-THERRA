using UnityEngine;

public abstract class OrbitalObject : MonoBehaviour
{
    public Transform centralObject;
    public float orbitSpeed = 10f;
    public float orbitRadius = 5f;

    protected Vector3 orbitAxis = Vector3.up;

    // Método abstracto para implementar el comportamiento específico de la órbita
    public abstract void Orbit();

    protected virtual void Update()
    {
        if (centralObject != null)
        {
            Orbit();
        }
    }
}
