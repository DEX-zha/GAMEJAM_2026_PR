using UnityEngine;
using UnityEngine.Rendering;

/// <summary>
/// Drives a local Volume placed in front of a door: the more the door is open, the more the
/// room's ambiance spills into the corridor. Works with any door system that rotates the door transform.
/// </summary>
[RequireComponent(typeof(Volume))]
public class DoorBlendVolume : MonoBehaviour
{
    public Transform door;
    [Tooltip("Door angle (degrees) at which the ambiance is fully spilled.")]
    public float fullOpenAngle = 70f;
    [Range(0f, 1f)] public float maxWeight = 0.85f;
    [Tooltip("Seconds to reach the target weight.")]
    public float smoothTime = 0.35f;

    Volume _volume;
    Quaternion _closed;
    float _weight, _vel;

    void Awake()
    {
        _volume = GetComponent<Volume>();
        if (door) _closed = door.localRotation;
        _volume.weight = 0f;
    }

    void Update()
    {
        if (!door) return;
        float open = Mathf.Clamp01(Quaternion.Angle(_closed, door.localRotation) / Mathf.Max(1f, fullOpenAngle));
        open = open * open * (3f - 2f * open);
        _weight = Mathf.SmoothDamp(_weight, open * maxWeight, ref _vel, smoothTime);
        _volume.weight = _weight;
    }
}
