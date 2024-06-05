using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraOffset : MonoBehaviour
{
    public Transform Player;
    public Vector3 offset;
    void Start()
    {
        offset = transform.position - Player.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Player.position + offset;
    }
}
