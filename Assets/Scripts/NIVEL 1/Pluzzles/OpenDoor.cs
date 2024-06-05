using UnityEngine;

public class OpenDoor : MonoBehaviour
{
    public Vector3 openPosition;
    public float openSpeed = 2f;
    private Vector3 closedPosition;
    private bool isOpening = false;
    bool abierto = false;
    private PuzzleChecker _puzzleChecker;

    void Start()
    {
        _puzzleChecker = new PuzzleChecker(abierto);
        closedPosition = transform.position;
    }

    void Update()
    {
        if (isOpening)
        {
            // Mover la puerta hacia la posición abierta
            transform.position = Vector3.Lerp(transform.position, openPosition, Time.deltaTime * openSpeed);

            // Si la puerta ha llegado a la posición abierta, detener el movimiento
            if (Vector3.Distance(transform.position, openPosition) < 0.01f)
            {
                transform.position = openPosition;
                isOpening = false;
            }
        }
    }

    public void Open(bool abierto)
    {
        if (abierto==true) 
        {
            isOpening = true;
            //cambio de escena

        }
        
        
    }
}
