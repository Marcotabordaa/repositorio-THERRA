using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class ToneGenerator : MonoBehaviour
{
    private AudioSource audioSource;
    private float sampleRate = 44100;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.loop = false;
    }

    public void PlayTone(float frequency, float duration)
    {
        int samples = Mathf.FloorToInt(sampleRate * duration);
        float[] wave = new float[samples];
        float increment = frequency * 2 * Mathf.PI / sampleRate;
        float phase = 0;

        for (int i = 0; i < samples; i++)
        {
            wave[i] = Mathf.Sin(phase);
            phase += increment;
        }

        AudioClip audioClip = AudioClip.Create("Tone", samples, 1, (int)sampleRate, false);
        audioClip.SetData(wave, 0);
        audioSource.clip = audioClip;
        audioSource.Play();
    }
}
