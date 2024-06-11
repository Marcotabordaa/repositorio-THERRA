using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TrayectoriaOndas : MonoBehaviour
{
    public Transform jugador;
    public float velocidad = 3f;
    public float amplitud = 10f;
    public float frecuencia = 3f;
    private float _contador = 0.0f;
    private float _posicionInicialY;

    // Start is called before the first frame update
    void Start()
    {
        _posicionInicialY = this.transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        _contador += frecuencia * Time.deltaTime;

        // Actualiza la posición de la onda en el eje Z y Y
        Vector3 direccion = jugador.position - transform.position;
        Vector3 direccionNormal = direccion.normalized;

        // Calcula la nueva posición del objeto
        Vector3 nuevaPosicion = transform.position + direccionNormal * velocidad * Time.deltaTime;
        float sinY = _posicionInicialY + amplitud * Mathf.Sin(_contador);

        // Mueve el objeto a la nueva posición
        transform.position = new Vector3(
            nuevaPosicion.x,
            sinY,
            nuevaPosicion.z
        );

    }

    
}
