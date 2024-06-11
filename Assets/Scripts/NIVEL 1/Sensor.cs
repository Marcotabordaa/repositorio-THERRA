using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(LineRenderer))]
public class Sensor : MonoBehaviour
{
    [Header("Configuración del Sensor")]
    [Tooltip("Define el tamaño inicial del sensor. Si lo desea, en el resto de las configuraciones podrá dejarlo fijo o que cambie dinámicamente de tamaño.")]
    public float tamañoSensor = 10;
    [Tooltip("El material que se le aplicará al sensor.")]
    public Material material;
    [Tooltip("El sonido FX que se le aplicará al sensor.")]
    public AudioClip sonidofx;
    [Tooltip("Define el grosor de línea con el que se pintará.")]
    public float lineaGrosor = 0.05f;
    [Tooltip("Define la velocidad de variación del tamaño del sensor.")]
    public float velocidadEscalado = 0.5f;
    [Tooltip("Define la frecuencia de sombreado que tendrá el sensor. Es la unidad de interlineado que se usará para dibujar.")]
    public float frecuenciaSombreado = 1f;
    [Tooltip("Tiempo de vida que estará activo el sensor. (Aplica para las formas de propagación: Bucle y Fijo)")]
    public float tiempo = 10;
    [Tooltip("Define el Tipo de Dibujo del Sensor. Puede ser de tipo Caja Sombreada o Plano Sombreado.")]
    public TipoDibujo tipoDibujo;

    [Space(5)]

    [Header("Forma de Propagación")]
    [Tooltip("Si está activado, el sensor se propagará expandiéndose solamente como una ráfaga. Su tiempo de vida será mientras no alcance el tamaño límite del sensor.")]
    public bool rafaga;
    [Tooltip("Si está activado, el tamaño del sensor cambiará de forma dinámica, expandiéndose y contrayéndose continuamente como un bucle. Su tiempo de vida estará marcado por la variable tiempo.")]
    public bool bucle;
    [Tooltip("Si está activado, el tamaño del sensor siempre será fijo. Su tiempo de vida estará marcado por la variable tiempo.")]
    public bool fijo;

    [Space(5)]

    [Header("Configuración de Ráfaga")]
    [Tooltip("Tamaño máximo límite del sensor. Define hasta donde puede crecer el sensor.")]
    public float tamañoSensorLimite = 40f;

    [Space(5)]

    [Header("Configuración de Bucle")]
    [Tooltip("Define el valor con el que variará el tamaño del sensor.")]
    public float valorBarrido = 10;

    private LineRenderer _lineRenderer;
    private BoxCollider _boxCollider;
    private AudioSource _audioSource;
    private float _barridoSuperior;
    private float _barridoInferior;
    private float _scale;
    private bool _estoyExpandiendo = true;
    private float _diferenciaDeAjuste = 0f;

    // Variables para opciones extras
    private float _tamañoSensorRestaurar;

    private void Start()
    {
        _tamañoSensorRestaurar = tamañoSensor;
    }

    private void OnEnable()
    {
        _lineRenderer = GetComponent<LineRenderer>();
        _boxCollider = GetComponent<BoxCollider>();
        _audioSource = GetComponent<AudioSource>();

        _audioSource.clip = sonidofx;
        _boxCollider.isTrigger = true;
        _lineRenderer.startWidth = lineaGrosor;
        _lineRenderer.material = material;
        _lineRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;

        _barridoSuperior = tamañoSensor + valorBarrido;
        _barridoInferior = tamañoSensor - valorBarrido;
        _scale = tamañoSensor;

        if (bucle)
        {
            Invoke("DesactivarSensor", tiempo);
            _audioSource.loop = true;
        }
        else if (fijo)
        {
            Invoke("DesactivarSensor", tiempo);
            _audioSource.loop = false;
        }
        else if (rafaga)
        {
            _audioSource.loop = false;
        }
        _audioSource.Play();
    }

    private void FixedUpdate()
    {
        transform.rotation = Quaternion.identity;
        _boxCollider.size = tipoDibujo == TipoDibujo.Caja_Sombreada ? new Vector3(tamañoSensor, tamañoSensor, tamañoSensor) : new Vector3(tamañoSensor, 0.5f, tamañoSensor);
        _boxCollider.center = tipoDibujo == TipoDibujo.Caja_Sombreada ? new Vector3(0, tamañoSensor / 2, 0) : new Vector3(0, 0.25f, 0);

        List<Vector3> posiciones = tipoDibujo == TipoDibujo.Caja_Sombreada ? DibujarCajaSombreada(transform.position) : DibujarPlanoSombreado(transform.position, 'x', 'z', true, true);

        _lineRenderer.positionCount = posiciones.Count;
        _lineRenderer.SetPositions(posiciones.ToArray());

        if (bucle)
        {
            if (Mathf.Abs(_scale - _barridoSuperior) < 1 || Mathf.Abs(_scale - _barridoInferior) < 1)
            {
                _estoyExpandiendo = !_estoyExpandiendo;
            }

            if (_estoyExpandiendo)
            {
                _scale = Mathf.Lerp(_scale, _barridoSuperior, velocidadEscalado * Time.deltaTime);
            }
            else
            {
                _scale = Mathf.Lerp(_scale, _barridoInferior, velocidadEscalado * Time.deltaTime);
            }

            tamañoSensor = _scale;
        }
        else if (rafaga)
        {
            if (Mathf.Abs(_scale - tamañoSensorLimite) < 1)
            {
                DesactivarSensor();
            }
            else if (tamañoSensor <= tamañoSensorLimite)
            {
                _scale = Mathf.Lerp(_scale, tamañoSensorLimite, velocidadEscalado * Time.deltaTime);
            }

            tamañoSensor = _scale;
        }
    }

    private void DesactivarSensor()
    {
        tamañoSensor = _tamañoSensorRestaurar;
        _scale = tamañoSensor;
        gameObject.SetActive(false);
    }

    private List<Vector3> DibujarCajaSombreada(Vector3 posicion)
    {
        List<Vector3> posiciones = new List<Vector3>();
        List<Vector3> planoXZ1 = DibujarPlanoSombreado(posicion, 'x', 'z', true, true);
        List<Vector3> planoXZ2 = DibujarPlanoSombreado(posicion, 'x', 'z', true, false);
        List<Vector3> planoZY1 = DibujarPlanoSombreado(posicion, 'z', 'y', true, true);
        List<Vector3> planoZY2 = DibujarPlanoSombreado(posicion, 'z', 'y', false, true);
        List<Vector3> planoXY1 = DibujarPlanoSombreado(posicion, 'x', 'y', true, true);
        List<Vector3> planoXY2 = DibujarPlanoSombreado(posicion, 'x', 'y', false, true);

        posiciones.AddRange(planoXZ1);
        posiciones.Add(planoXZ1[0]);

        posiciones.AddRange(planoZY1);
        posiciones.Add(planoZY1[3]);

        posiciones.AddRange(planoXY1);
        posiciones.Add(planoXY1[3]);

        posiciones.AddRange(planoZY2);
        posiciones.Add(planoZY2[3]);

        posiciones.AddRange(planoXY2);
        posiciones.Add(planoXY2[2]);
        posiciones.Add(planoZY1[2]);

        posiciones.AddRange(planoXZ2);

        return posiciones;
    }

    private List<Vector3> DibujarPlanoSombreado(Vector3 posicion, char eje1, char eje2, bool eje1PositivoDerecha, bool eje2PositivoArriba)
    {
        List<Vector3> puntos = new List<Vector3>();
        Vector3 punto1 = new Vector3();
        Vector3 punto2 = new Vector3();
        Vector3 punto3 = new Vector3();
        Vector3 punto4 = new Vector3();

        if (eje1 == 'x' && eje2 == 'z')
        {
            if (eje1PositivoDerecha && eje2PositivoArriba)
            {
                punto1 = new Vector3(posicion.x - tamañoSensor / 2, posicion.y, posicion.z - tamañoSensor / 2);
                punto2 = new Vector3(posicion.x - tamañoSensor / 2, posicion.y, posicion.z + tamañoSensor / 2);
                punto3 = new Vector3(posicion.x + tamañoSensor / 2, posicion.y, posicion.z + tamañoSensor / 2);
                punto4 = new Vector3(posicion.x + tamañoSensor / 2, posicion.y, posicion.z - tamañoSensor / 2);
            }
            else if (eje1PositivoDerecha && !eje2PositivoArriba)
            {
                punto1 = new Vector3(posicion.x - tamañoSensor / 2, posicion.y + tamañoSensor, posicion.z + tamañoSensor / 2);
                punto2 = new Vector3(posicion.x - tamañoSensor / 2, posicion.y + tamañoSensor, posicion.z - tamañoSensor / 2);
                punto3 = new Vector3(posicion.x + tamañoSensor / 2, posicion.y + tamañoSensor, posicion.z - tamañoSensor / 2);
                punto4 = new Vector3(posicion.x + tamañoSensor / 2, posicion.y + tamañoSensor, posicion.z + tamañoSensor / 2);
            }
        }
        else if (eje1 == 'x' && eje2 == 'y')
        {
            if (eje1PositivoDerecha && eje2PositivoArriba)
            {
                punto1 = new Vector3(posicion.x - tamañoSensor / 2, posicion.y, posicion.z - tamañoSensor / 2);
                punto2 = new Vector3(posicion.x - tamañoSensor / 2, posicion.y + tamañoSensor, posicion.z - tamañoSensor / 2);
                punto3 = new Vector3(posicion.x + tamañoSensor / 2, posicion.y + tamañoSensor, posicion.z - tamañoSensor / 2);
                punto4 = new Vector3(posicion.x + tamañoSensor / 2, posicion.y, posicion.z - tamañoSensor / 2);
            }
            else if (!eje1PositivoDerecha && eje2PositivoArriba)
            {
                punto1 = new Vector3(posicion.x - tamañoSensor / 2, posicion.y, posicion.z + tamañoSensor / 2);
                punto2 = new Vector3(posicion.x - tamañoSensor / 2, posicion.y + tamañoSensor, posicion.z + tamañoSensor / 2);
                punto3 = new Vector3(posicion.x + tamañoSensor / 2, posicion.y + tamañoSensor, posicion.z + tamañoSensor / 2);
                punto4 = new Vector3(posicion.x + tamañoSensor / 2, posicion.y, posicion.z + tamañoSensor / 2);
            }
        }
        else if (eje1 == 'z' && eje2 == 'y')
        {
            if (eje1PositivoDerecha && eje2PositivoArriba)
            {
                punto1 = new Vector3(posicion.x - tamañoSensor / 2, posicion.y, posicion.z - tamañoSensor / 2);
                punto2 = new Vector3(posicion.x - tamañoSensor / 2, posicion.y + tamañoSensor, posicion.z - tamañoSensor / 2);
                punto3 = new Vector3(posicion.x - tamañoSensor / 2, posicion.y + tamañoSensor, posicion.z + tamañoSensor / 2);
                punto4 = new Vector3(posicion.x - tamañoSensor / 2, posicion.y, posicion.z + tamañoSensor / 2);
            }
            else if (!eje1PositivoDerecha && eje2PositivoArriba)
            {
                punto1 = new Vector3(posicion.x + tamañoSensor / 2, posicion.y, posicion.z - tamañoSensor / 2);
                punto2 = new Vector3(posicion.x + tamañoSensor / 2, posicion.y + tamañoSensor, posicion.z - tamañoSensor / 2);
                punto3 = new Vector3(posicion.x + tamañoSensor / 2, posicion.y + tamañoSensor, posicion.z + tamañoSensor / 2);
                punto4 = new Vector3(posicion.x + tamañoSensor / 2, posicion.y, posicion.z + tamañoSensor / 2);
            }
        }

        puntos.Add(punto1);
        puntos.Add(punto2);
        puntos.Add(punto3);
        puntos.Add(punto4);

        return puntos;
    }

    public enum TipoDibujo
    {
        Caja_Sombreada,
        Plano_Sombreado
    }
}

