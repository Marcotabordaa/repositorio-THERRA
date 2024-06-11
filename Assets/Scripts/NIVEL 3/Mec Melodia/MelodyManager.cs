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
    public ExplosionController aticaExplosionController;
    public ExplosionController astarothExplosionController;
    public Renderer centralPlanetRenderer;
    public ToneGenerator toneGenerator;
    public MelodyUIManager melodyUIManager; // Referencia al MelodyUIManager
    public ScoreManager scoreManager; // Referencia al ScoreManager
    public GameObject winPanel; // Panel de victoria
    public GameObject losePanel; // Panel de derrota
    private int currentMelodyIndex = 0;
    private bool waitingForPlayerInput = false;
    private Dictionary<KeyCode, float> keyFrequencies;
    private int redShotCount = 0;
    private int blueShotCount = 0;

    void Start()
    {
        melodies = new List<Melody>
        {
            new Melody("Melody One", new List<KeyCode> { KeyCode.UpArrow, KeyCode.LeftArrow, KeyCode.RightArrow, KeyCode.UpArrow, KeyCode.LeftArrow, KeyCode.RightArrow }),
            new Melody("Melody Two", new List<KeyCode> { KeyCode.LeftArrow, KeyCode.UpArrow, KeyCode.RightArrow, KeyCode.LeftArrow, KeyCode.UpArrow, KeyCode.RightArrow }),
            new Melody("Melody Three", new List<KeyCode> { KeyCode.UpArrow, KeyCode.LeftArrow, KeyCode.A, KeyCode.UpArrow, KeyCode.LeftArrow, KeyCode.A }),
            new Melody("Melody Four", new List<KeyCode> { KeyCode.A, KeyCode.LeftArrow, KeyCode.RightArrow, KeyCode.A, KeyCode.LeftArrow, KeyCode.RightArrow })
        };

        keyFrequencies = new Dictionary<KeyCode, float>
        {
            { KeyCode.LeftArrow, 261.63f }, // C
            { KeyCode.UpArrow, 293.66f }, // D
            { KeyCode.RightArrow, 329.63f }, // E
            { KeyCode.DownArrow, 349.23f }, // F
            { KeyCode.A, 392.00f } // G
        };

        StartCoroutine(ShowMelodySequence());
    }

    void Update()
    {
        if (waitingForPlayerInput && Input.anyKeyDown)
        {
            KeyCode keyPressed = GetKeyPressed();
            if (keyPressed != KeyCode.None)
            {
                if (keyFrequencies.ContainsKey(keyPressed))
                {
                    toneGenerator.PlayTone(keyFrequencies[keyPressed], 0.5f);
                }

                currentInput.Add(keyPressed);
                if (currentInput.Count >= 6) // Esperar a que el jugador ingrese al menos seis teclas
                {
                    CheckMelody();
                }
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

    private void CheckMelody()
    {
        bool matched = false;

        foreach (var melody in melodies)
        {
            if (currentInput.Count != melody.Notes.Count)
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
                Debug.Log("Played melody: " + melody.Name);
                aticaExplosionController.TriggerExplosion();
                redShotCount++;
                blueShotCount = Mathf.Max(0, blueShotCount - 1);
                StartCoroutine(UpdateCentralPlanetColor());
                matched = true;

                // Incrementar los puntos del jugador cuando acierta
                scoreManager.AddPlayerPoints(10); // Por ejemplo, 10 puntos por melodía acertada
                break;
            }
        }

        if (!matched)
        {
            astarothExplosionController.TriggerExplosion();
            blueShotCount++;
            redShotCount = Mathf.Max(0, redShotCount - 1);
            StartCoroutine(UpdateCentralPlanetColor());

            // Incrementar los puntos del rojo cuando el jugador falla
            scoreManager.AddRedPoints(10); // Por ejemplo, 5 puntos por melodía fallada
        }

        currentInput.Clear();
        waitingForPlayerInput = false;

        // Verificar si se ha alcanzado el límite de puntuación
        if (scoreManager.IsScoreLimitReached())
        {
            if (scoreManager.IsPlayerWinner())
            {
                winPanel.SetActive(true); // Mostrar el panel de victoria
            }
            else
            {
                losePanel.SetActive(true); // Mostrar el panel de derrota
            }
        }
        else
        {
            // Incrementar el índice de la melodía para pasar a la siguiente
            currentMelodyIndex++;
            if (currentMelodyIndex < melodies.Count)
            {
                StartCoroutine(ShowNextMelody());
            }
            else
            {
                currentMelodyIndex = 0; // Reiniciar el índice si se han completado todas las melodías
                StartCoroutine(ShowNextMelody());
            }
        }
    }

    private IEnumerator ShowMelodySequence()
    {
        if (currentMelodyIndex < melodies.Count)
        {
            melodyUIManager.ShowMelody(currentMelodyIndex);
            yield return new WaitForSeconds(3f);
            melodyUIManager.HideAllMelodies();
            waitingForPlayerInput = true;
        }
    }

    private IEnumerator ShowNextMelody()
    {
        yield return new WaitForSeconds(1f); // Pequeña pausa antes de mostrar la siguiente melodía
        StartCoroutine(ShowMelodySequence());
    }

    private IEnumerator UpdateCentralPlanetColor()
    {
        float redValue = Mathf.Clamp01(redShotCount / 5f);
        float blueValue = Mathf.Clamp01(blueShotCount / 5f);
        Color newColor = new Color(blueValue, 0, redValue);
        yield return new WaitForSeconds(1f);
        centralPlanetRenderer.material.color = newColor;
    }
}
