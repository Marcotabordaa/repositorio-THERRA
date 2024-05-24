using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Personaje : MonoBehaviour
{
    public float velocidad = 10;
    public float velRotacion = 90;

    public static Personaje singleton;

    private void Awake()
    {
        singleton = this;
    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(0, 0, Input.GetAxis("Vertical") * velocidad * Time.deltaTime);
        transform.Rotate(0, Input.GetAxis("Horizontal") * velRotacion * Time.deltaTime, 0);

    }
}