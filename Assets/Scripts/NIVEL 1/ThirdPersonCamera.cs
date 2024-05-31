using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    public Transform target; // El objetivo a seguir (el personaje)
    public float distance = 5.0f; // Distancia desde el objetivo
    public float xSpeed = 120.0f; // Velocidad de rotación en el eje X
    public float ySpeed = 120.0f; // Velocidad de rotación en el eje Y

    public float yMinLimit = -20f; // Límite mínimo de rotación en el eje Y
    public float yMaxLimit = 80f; // Límite máximo de rotación en el eje Y

    private float x = 0.0f; // Rotación actual en el eje X
    private float y = 0.0f; // Rotación actual en el eje Y

    private bool isRotating = false; // Indica si se está rotando la cámara

    void Start()
    {
        Vector3 angles = transform.eulerAngles;
        x = angles.y;
        y = angles.x;
    }

    void Update()
    {
        // Activar o desactivar la rotación de la cámara con el botón derecho del mouse
        if (Input.GetMouseButtonDown(1))
        {
            isRotating = true;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else if (Input.GetMouseButtonUp(1))
        {
            isRotating = false;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    void LateUpdate()
    {
        if (target && isRotating)
        {
            // Obtener la entrada del mouse
            x += Input.GetAxis("Mouse X") * xSpeed * Time.deltaTime;
            y -= Input.GetAxis("Mouse Y") * ySpeed * Time.deltaTime;

            // Limitar la rotación en el eje Y
            y = ClampAngle(y, yMinLimit, yMaxLimit);

            // Calcular la rotación
            Quaternion rotation = Quaternion.Euler(y, x, 0);

            // Calcular la posición de la cámara
            Vector3 position = rotation * new Vector3(0.0f, 0.0f, -distance) + target.position;

            // Establecer la rotación y posición de la cámara
            transform.rotation = rotation;
            transform.position = position;
        }
    }

    // Método para limitar el ángulo de rotación
    private static float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360F)
            angle += 360F;
        if (angle > 360F)
            angle -= 360F;
        return Mathf.Clamp(angle, min, max);
    }
}
