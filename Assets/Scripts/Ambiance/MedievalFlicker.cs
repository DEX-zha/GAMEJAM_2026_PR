using UnityEngine;

/// <summary>
/// Fire-like flicker for a Light (lantern, torch, candle). Reacts to WindAmbience gusts:
/// stronger wind = more nervous flame.
/// </summary>
[RequireComponent(typeof(Light))]
public class MedievalFlicker : MonoBehaviour
{
    [Tooltip("Base intensity; the flicker oscillates around it.")]
    public float baseIntensity = 3f;
    [Range(0f, 0.6f)] public float flickerAmount = 0.18f;
    public float flickerSpeed = 7f;
    [Tooltip("Extra flicker added during wind gusts.")]
    [Range(0f, 0.6f)] public float windExtra = 0.2f;
    [Tooltip("Small wobble of the light position (metres) to make shadows dance.")]
    public float positionJitter = 0.015f;
    [Tooltip("Warm colour shift between calm and strong flame.")]
    public Color calmColor = new Color(1f, 0.62f, 0.30f);
    public Color hotColor = new Color(1f, 0.78f, 0.45f);

    Light _light;
    Vector3 _basePos;
    float _seed;

    void Awake()
    {
        _light = GetComponent<Light>();
        _basePos = transform.localPosition;
        _seed = Random.value * 100f;
    }

    void Update()
    {
        float t = Time.time * flickerSpeed + _seed;
        float gust = WindAmbience.Gust;
        float amount = flickerAmount + windExtra * gust;
        float n = Mathf.PerlinNoise(t, _seed) * 0.65f + Mathf.PerlinNoise(t * 2.7f, _seed + 3f) * 0.35f; // 0..1
        float k = 1f + (n - 0.5f) * 2f * amount;
        _light.intensity = baseIntensity * k;
        _light.color = Color.Lerp(calmColor, hotColor, n);
        if (positionJitter > 0f)
        {
            float j = positionJitter * (1f + gust);
            transform.localPosition = _basePos + new Vector3(
                (Mathf.PerlinNoise(t * 0.8f, 11f) - 0.5f) * j,
                (Mathf.PerlinNoise(t * 0.8f, 23f) - 0.5f) * j,
                (Mathf.PerlinNoise(t * 0.8f, 37f) - 0.5f) * j);
        }
    }
}
