using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    public Transform target; // El objetivo a seguir (el personaje)
    public float distance = 5.0f; // Distancia desde el objetivo
    public float xSpeed = 120.0f; // Velocidad de rotaci�n en el eje X
    public float ySpeed = 120.0f; // Velocidad de rotaci�n en el eje Y

    public float yMinLimit = -20f; // L�mite m�nimo de rotaci�n en el eje Y
    public float yMaxLimit = 80f; // L�mite m�ximo de rotaci�n en el eje Y

    private float x = 0.0f; // Rotaci�n actual en el eje X
    private float y = 0.0f; // Rotaci�n actual en el eje Y

    private bool isRotating = false; // Indica si se est� rotando la c�mara

    void Start()
    {
        Vector3 angles = transform.eulerAngles;
        x = angles.y;
        y = angles.x;
    }

    void Update()
    {
        // Activar o desactivar la rotaci�n de la c�mara con el bot�n derecho del mouse
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

            // Limitar la rotaci�n en el eje Y
            y = ClampAngle(y, yMinLimit, yMaxLimit);

            // Calcular la rotaci�n
            Quaternion rotation = Quaternion.Euler(y, x, 0);

            // Calcular la posici�n de la c�mara
            Vector3 position = rotation * new Vector3(0.0f, 0.0f, -distance) + target.position;

            // Establecer la rotaci�n y posici�n de la c�mara
            transform.rotation = rotation;
            transform.position = position;
        }
    }

    // M�todo para limitar el �ngulo de rotaci�n
    private static float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360F)
            angle += 360F;
        if (angle > 360F)
            angle -= 360F;
        return Mathf.Clamp(angle, min, max);
    }
}
