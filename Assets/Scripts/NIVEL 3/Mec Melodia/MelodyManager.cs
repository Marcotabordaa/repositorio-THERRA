using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Melody
{
    public string Name;
    public List<KeyCode> Notes;

    public Melody(string name, List<KeyCode> notes)
    {
        Name = name;
        Notes = notes;
    }
}

public class MelodyManager : MonoBehaviour
{
    public List<Melody> melodies;
    private List<KeyCode> currentInput = new List<KeyCode>();
    public ExplosionController redExplosionController;
    public ExplosionController blueExplosionController;
    public Renderer centralPlanetRenderer;
    public ToneGenerator toneGenerator; // Referencia al ToneGenerator
    private int redShotCount = 0;
    private int blueShotCount = 0;
    private Dictionary<KeyCode, float> keyFrequencies; // Mapa de teclas a frecuencias

    void Start()
    {
        melodies = new List<Melody>
        {
            new Melody("Red Melody", new List<KeyCode> { KeyCode.LeftArrow, KeyCode.UpArrow, KeyCode.RightArrow, KeyCode.LeftArrow, KeyCode.UpArrow, KeyCode.RightArrow }),
            new Melody("Blue Melody", new List<KeyCode> { KeyCode.UpArrow, KeyCode.LeftArrow, KeyCode.RightArrow, KeyCode.UpArrow, KeyCode.LeftArrow, KeyCode.RightArrow })
        };

        keyFrequencies = new Dictionary<KeyCode, float> // Asignar frecuencias a cada tecla
        {
            { KeyCode.LeftArrow, 261.63f }, // C
            { KeyCode.UpArrow, 293.66f }, // D
            { KeyCode.RightArrow, 329.63f }, // E
            { KeyCode.DownArrow, 349.23f }, // F
            { KeyCode.A, 392.00f } // G
        };
    }

    void Update()
    {
        if (Input.anyKeyDown)
        {
            KeyCode keyPressed = GetKeyPressed();
            if (keyPressed != KeyCode.None)
            {
                if (keyFrequencies.ContainsKey(keyPressed))
                {
                    toneGenerator.PlayTone(keyFrequencies[keyPressed], 0.5f); // Reproducir tono por 0.5 segundos
                }

                currentInput.Add(keyPressed);
                CheckMelodies();
            }
        }
    }

    private KeyCode GetKeyPressed()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow)) return KeyCode.LeftArrow;
        if (Input.GetKeyDown(KeyCode.UpArrow)) return KeyCode.UpArrow;
        if (Input.GetKeyDown(KeyCode.RightArrow)) return KeyCode.RightArrow;
        if (Input.GetKeyDown(KeyCode.DownArrow)) return KeyCode.DownArrow;
        if (Input.GetKeyDown(KeyCode.A)) return KeyCode.A;
        return KeyCode.None;
    }

    private void CheckMelodies()
    {
        bool matched = false;

        foreach (var melody in melodies)
        {
            if (currentInput.Count > melody.Notes.Count)
            {
                continue;
            }

            bool isMatch = true;
            for (int i = 0; i < currentInput.Count; i++)
            {
                if (currentInput[i] != melody.Notes[i])
                {
                    isMatch = false;
                    break;
                }
            }

            if (isMatch)
            {
                if (currentInput.Count == melody.Notes.Count)
                {
                    Debug.Log("Played melody: " + melody.Name);
                    if (melody.Name == "Red Melody")
                    {
                        redExplosionController.TriggerExplosion();
                        redShotCount++;
                        blueShotCount = Mathf.Max(0, blueShotCount - 1);
                    }
                    else if (melody.Name == "Blue Melody")
                    {
                        blueExplosionController.TriggerExplosion();
                        blueShotCount++;
                        redShotCount = Mathf.Max(0, redShotCount - 1);
                    }
                    StartCoroutine(UpdateCentralPlanetColor());
                    currentInput.Clear();
                }
                matched = true;
                break;
            }
        }

        if (!matched && currentInput.Count > 0)
        {
            currentInput.Clear();
        }
    }

    private IEnumerator UpdateCentralPlanetColor()
    {
        float redValue = Mathf.Clamp01(redShotCount / 5f);
        float blueValue = Mathf.Clamp01(blueShotCount / 5f);
        Color newColor = new Color(blueValue, 0, redValue); // Cambio de valores para corregir colores
        yield return new WaitForSeconds(1f); // Delay para coincidir con la finalización de las partículas
        centralPlanetRenderer.material.color = newColor;
    }
}



