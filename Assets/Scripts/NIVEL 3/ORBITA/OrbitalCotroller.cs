using UnityEngine;

public class OrbitController : OrbitalObject
{
    public override void Orbit()
    {
        // Rotar alrededor del objeto central
        transform.RotateAround(centralObject.position, orbitAxis, orbitSpeed * Time.deltaTime);

        // Mantener la distancia (radio de la órbita) desde el objeto central
        Vector3 desiredPosition = (transform.position - centralObject.position).normalized * orbitRadius + centralObject.position;
        transform.position = desiredPosition;
    }
}
