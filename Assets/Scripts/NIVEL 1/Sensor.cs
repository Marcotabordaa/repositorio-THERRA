using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(LineRenderer))]
public class Sensor : MonoBehaviour
{
    [Header("Configuraci?n del Sensor")]
    [Tooltip("Define el tama?o inicial del sensor. Si lo desea, en el resto de las configuraciones podr? dejarlo fijo o que cambie din?micamente de tama?o.")]
    public float tamanoSensor = 10;
    [Tooltip("El material que se le aplicar? al sensor.")]
    public Material material;
    [Tooltip("El sonido FX que se le aplicar? al sensor.")]
    public AudioClip sonidofx;
    [Tooltip("Define el grosor de l?nea con el que se pintar?.")]
    public float lineaGrosor = 0.05f;
    [Tooltip("Define la velocidad de variaci?n del tama?o del sensor.")]
    public float velocidadEscalado = 0.5f;
    [Tooltip("Define la frecuencia de sombreado que tendr? el sensor. Es la unidad de interlineado que se usar? para dibujar.")]
    public float frecuenciaSombreado = 1f;
    [Tooltip("Tiempo de vida que estar? activo el sensor. (Aplica para las formas de propagaci?n: Bucle y Fijo)")]
    public float tiempo = 10;
    [Tooltip("Define el Tipo de Dibujo del Sensor. Puede ser de tipo Caja Sombreada o Plano Sombreado.")]
    public TipoDibujo tipoDibujo;

    [Space(5)]

    [Header("Forma de Propagaci?n")]
    [Tooltip("Si est? activado, el sensor se propagar? expandi?ndose solamente como una r?faga. Su tiempo de vida ser? mientras no alcance el tama?o l?mite del sensor.")]
    public bool rafaga;
    [Tooltip("Si est? activado, el tama?o del sensor cambiar? de forma din?mica, expandi?ndose y contray?ndose continuamente como un bucle. Su tiempo de vida estar? marcado por la variable tiempo.")]
    public bool bucle;
    [Tooltip("Si est? activado, el tama?o del sensor siempre ser? fijo. Su tiempo de vida estar? marcado por la variable tiempo.")]
    public bool fijo;

    [Space(5)]

    [Header("Configuraci?n de R?faga")]
    [Tooltip("Tama?o m?ximo l?mite del sensor. Define hasta donde puede crecer el sensor.")]
    public float tamanoSensorLimite = 40f;

    [Space(5)]

    [Header("Configuraci?n de Bucle")]
    [Tooltip("Define el valor con el que variar? el tama?o del sensor.")]
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
    private float _tamanoSensorRestaurar;

    private void Start()
    {
        _tamanoSensorRestaurar = tamanoSensor;
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

        _barridoSuperior = tamanoSensor + valorBarrido;
        _barridoInferior = tamanoSensor - valorBarrido;
        _scale = tamanoSensor;

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
        _boxCollider.size = tipoDibujo == TipoDibujo.Caja_Sombreada ? new Vector3(tamanoSensor, tamanoSensor, tamanoSensor) : new Vector3(tamanoSensor, 0.5f, tamanoSensor);
        _boxCollider.center = tipoDibujo == TipoDibujo.Caja_Sombreada ? new Vector3(0, tamanoSensor / 2, 0) : new Vector3(0, 0.25f, 0);

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

            tamanoSensor = _scale;
        }
        else if (rafaga)
        {
            if (Mathf.Abs(_scale - tamanoSensorLimite) < 1)
            {
                DesactivarSensor();
            }
            else if (tamanoSensor <= tamanoSensorLimite)
            {
                _scale = Mathf.Lerp(_scale, tamanoSensorLimite, velocidadEscalado * Time.deltaTime);
            }

            tamanoSensor = _scale;
        }
    }

    private void DesactivarSensor()
    {
        tamanoSensor = _tamanoSensorRestaurar;
        _scale = tamanoSensor;
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
                punto1 = new Vector3(posicion.x - tamanoSensor / 2, posicion.y, posicion.z - tamanoSensor / 2);
                punto2 = new Vector3(posicion.x - tamanoSensor / 2, posicion.y, posicion.z + tamanoSensor / 2);
                punto3 = new Vector3(posicion.x + tamanoSensor / 2, posicion.y, posicion.z + tamanoSensor / 2);
                punto4 = new Vector3(posicion.x + tamanoSensor / 2, posicion.y, posicion.z - tamanoSensor / 2);
            }
            else if (eje1PositivoDerecha && !eje2PositivoArriba)
            {
                punto1 = new Vector3(posicion.x - tamanoSensor / 2, posicion.y + tamanoSensor, posicion.z + tamanoSensor / 2);
                punto2 = new Vector3(posicion.x - tamanoSensor / 2, posicion.y + tamanoSensor, posicion.z - tamanoSensor / 2);
                punto3 = new Vector3(posicion.x + tamanoSensor / 2, posicion.y + tamanoSensor, posicion.z - tamanoSensor / 2);
                punto4 = new Vector3(posicion.x + tamanoSensor / 2, posicion.y + tamanoSensor, posicion.z + tamanoSensor / 2);
            }
        }
        else if (eje1 == 'x' && eje2 == 'y')
        {
            if (eje1PositivoDerecha && eje2PositivoArriba)
            {
                punto1 = new Vector3(posicion.x - tamanoSensor / 2, posicion.y, posicion.z - tamanoSensor / 2);
                punto2 = new Vector3(posicion.x - tamanoSensor / 2, posicion.y + tamanoSensor, posicion.z - tamanoSensor / 2);
                punto3 = new Vector3(posicion.x + tamanoSensor / 2, posicion.y + tamanoSensor, posicion.z - tamanoSensor / 2);
                punto4 = new Vector3(posicion.x + tamanoSensor / 2, posicion.y, posicion.z - tamanoSensor / 2);
            }
            else if (!eje1PositivoDerecha && eje2PositivoArriba)
            {
                punto1 = new Vector3(posicion.x - tamanoSensor / 2, posicion.y, posicion.z + tamanoSensor / 2);
                punto2 = new Vector3(posicion.x - tamanoSensor / 2, posicion.y + tamanoSensor, posicion.z + tamanoSensor / 2);
                punto3 = new Vector3(posicion.x + tamanoSensor / 2, posicion.y + tamanoSensor, posicion.z + tamanoSensor / 2);
                punto4 = new Vector3(posicion.x + tamanoSensor / 2, posicion.y, posicion.z + tamanoSensor / 2);
            }
        }
        else if (eje1 == 'z' && eje2 == 'y')
        {
            if (eje1PositivoDerecha && eje2PositivoArriba)
            {
                punto1 = new Vector3(posicion.x - tamanoSensor / 2, posicion.y, posicion.z - tamanoSensor / 2);
                punto2 = new Vector3(posicion.x - tamanoSensor / 2, posicion.y + tamanoSensor, posicion.z - tamanoSensor / 2);
                punto3 = new Vector3(posicion.x - tamanoSensor / 2, posicion.y + tamanoSensor, posicion.z + tamanoSensor / 2);
                punto4 = new Vector3(posicion.x - tamanoSensor / 2, posicion.y, posicion.z + tamanoSensor / 2);
            }
            else if (!eje1PositivoDerecha && eje2PositivoArriba)
            {
                punto1 = new Vector3(posicion.x + tamanoSensor / 2, posicion.y, posicion.z - tamanoSensor / 2);
                punto2 = new Vector3(posicion.x + tamanoSensor / 2, posicion.y + tamanoSensor, posicion.z - tamanoSensor / 2);
                punto3 = new Vector3(posicion.x + tamanoSensor / 2, posicion.y + tamanoSensor, posicion.z + tamanoSensor / 2);
                punto4 = new Vector3(posicion.x + tamanoSensor / 2, posicion.y, posicion.z + tamanoSensor / 2);
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

