using UnityEngine;

/// <summary>
/// Procedural night wind for a room: generates a looping 3D wind sound (filtered noise with gusts),
/// exposes the current gust strength (0..1) for other effects (light flicker, particles) and drives an optional WindZone.
/// No audio asset needed.
/// </summary>
[RequireComponent(typeof(AudioSource))]
public class WindAmbience : MonoBehaviour
{
    /// <summary>Current gust strength (0..1), shared with flickering lights.</summary>
    public static float Gust { get; private set; }

    [Header("Sound")]
    [Range(0f, 1f)] public float volume = 0.35f;
    [Tooltip("Low-pass cutoff of the wind noise when calm / during a gust (Hz).")]
    public Vector2 cutoffCalmGust = new Vector2(250f, 900f);

    [Header("Gusts")]
    [Tooltip("Average seconds between gust peaks.")]
    public float gustPeriod = 6f;
    [Range(0f, 1f)] public float baseStrength = 0.25f;

    [Header("Optional links")]
    public WindZone windZone;
    public float windZoneMainCalm = 0.3f;
    public float windZoneMainGust = 1.4f;

    AudioSource _src;
    int _sampleRate;
    System.Random _rng;
    float _lp1, _lp2, _brown;
    volatile float _gustAudio;
    double _phase;

    void Awake()
    {
        _rng = new System.Random(12345);
        _sampleRate = AudioSettings.outputSampleRate;
        _src = GetComponent<AudioSource>();
        _src.clip = AudioClip.Create("ProceduralWind", _sampleRate * 4, 1, _sampleRate, true, OnAudioRead);
        _src.loop = true;
        if (!_src.isPlaying) _src.Play();
    }

    void Update()
    {
        // Layered Perlin noise -> smooth, irregular gusts
        float t = Time.time / Mathf.Max(0.5f, gustPeriod);
        float g = Mathf.PerlinNoise(t, 0.37f) * 0.7f + Mathf.PerlinNoise(t * 3.1f, 1.7f) * 0.3f;
        g = Mathf.Clamp01((g - 0.35f) * 1.8f);
        Gust = Mathf.Clamp01(baseStrength + (1f - baseStrength) * g * g);
        _gustAudio = Gust;

        if (windZone)
        {
            windZone.windMain = Mathf.Lerp(windZoneMainCalm, windZoneMainGust, Gust);
            windZone.windTurbulence = Mathf.Lerp(0.3f, 1.2f, Gust);
        }
    }

    void OnAudioRead(float[] data)
    {
        float gust = _gustAudio;
        float cutoff = Mathf.Lerp(cutoffCalmGust.x, cutoffCalmGust.y, gust);
        float a = 1f - Mathf.Exp(-2f * Mathf.PI * cutoff / Mathf.Max(8000, _sampleRate));
        float amp = volume * Mathf.Lerp(0.25f, 1f, gust);
        for (int i = 0; i < data.Length; i++)
        {
            float white = (float)(_rng.NextDouble() * 2.0 - 1.0);
            _brown = Mathf.Clamp(_brown + white * 0.02f, -1f, 1f) * 0.998f;
            _lp1 += a * (white - _lp1);
            _lp2 += a * (_lp1 - _lp2);
            // slow "whistle" modulation for texture
            _phase += 0.35 / _sampleRate;
            float whistle = 0.85f + 0.15f * Mathf.Sin((float)(_phase * 2.0 * Mathf.PI));
            data[i] = (_lp2 * 2.2f + _brown * 0.6f) * amp * whistle;
        }
    }
}
