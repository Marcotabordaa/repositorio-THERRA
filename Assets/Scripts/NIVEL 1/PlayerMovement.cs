using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;
    public float speed = 12f;
    public float gravity = -9.81f;
    public float jumpHeight = 3f;
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;
    public float turnSmoothTime = 0.1f;
    public float turnSmoothVelocity;
    public Transform cam;
    Vector3 velocity;
    bool isGrounded;



    void Update()
    {
        // Verificar si estamos en el suelo
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; // Para que el personaje no esté siempre aplicando gravedad al estar en el suelo
        }

        // Obtener las entradas del usuario
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;

        if  (move.magnitude >= 0.1)
        {
            float TargetAngle = Mathf.Atan2(move.x, move.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, TargetAngle, ref turnSmoothVelocity, turnSmoothTime);


            Vector3 moveDirection = Quaternion.Euler(0f, TargetAngle,0f)*Vector3.forward; ;    
           //transform.rotation= Quaternion.Euler(0f, angle, 0f);
            controller.Move( moveDirection.normalized* speed * Time.deltaTime);
        }
            
      

        // Verificar si el jugador está saltando
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
           
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        // Aplicar la gravedad al jugador
        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);
    }
}
