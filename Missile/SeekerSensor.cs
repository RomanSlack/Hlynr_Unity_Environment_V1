using UnityEngine;

/// Simple monopulse seeker with FOV, range, and g‑limit.
[AddComponentMenu("Simulation/Missile/Seeker Sensor")]
public sealed class SeekerSensor : MonoBehaviour
{
    [Tooltip("Target this seeker tries to track")]
    public Transform target;

    [Header("Limits")]
    [Range(5f, 90f)] public float halfFOVdeg = 30f;     // cone half‑angle
    public float maxRange = 200f;                       // metres
    public float maxTrackRateDegPerSec = 60f;           // g‑limit proxy

    public bool HasLock { get; private set; }

    Vector3 prevLOSdir;

    void FixedUpdate()
    {
        if (!target) { HasLock = false; return; }

        Vector3 toTarget = target.position - transform.position;
        float dist = toTarget.magnitude;
        if (dist > maxRange) { HasLock = false; return; }

        Vector3 losDir = toTarget / dist;
        float angle = Vector3.Angle(transform.forward, losDir);
        if (angle > halfFOVdeg) { HasLock = false; return; }

        // Track‑rate / g‑limit check
        if (prevLOSdir != Vector3.zero)
        {
            float trackRate = Vector3.Angle(prevLOSdir, losDir) / Time.fixedDeltaTime;
            if (trackRate > maxTrackRateDegPerSec)
            {
                HasLock = false; return;
            }
        }

        HasLock = true;
        prevLOSdir = losDir;
    }
}
