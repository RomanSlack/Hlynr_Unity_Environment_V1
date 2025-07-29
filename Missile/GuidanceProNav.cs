using UnityEngine;

/// Simple proportional‑navigation / pursuit guidance.
/// Attach to missile; assign Target via Inspector or tag “Target”.
[AddComponentMenu("Simulation/Missile/Guidance – Pro‑Nav")]
[RequireComponent(typeof(PIDAttitudeController))]
public sealed class GuidanceProNav : MonoBehaviour
{
    [Tooltip("Object the missile will pursue")]
    public Transform target;

    [Tooltip("Time (s) the missile should take to align with LOS")]
    public float timeToAlign = 0.5f;

    public Vector3 GetDesiredAccelBody() => lastAccelBody;
    Vector3 lastAccelBody;  // field



    PIDAttitudeController pid;
    SeekerSensor sensor;
    Rigidbody rb;

    void Awake()
    {

        pid = GetComponent<PIDAttitudeController>();
        sensor = GetComponent<SeekerSensor>();          // NEW
        rb = GetComponent<Rigidbody>();
        // If target not set, find first object tagged "Target"
        if (target == null)
        {
            var tgtObj = GameObject.FindGameObjectWithTag("Target");
            if (tgtObj) target = tgtObj.transform;
        }
    }

    void FixedUpdate()
    {
        if (!target || (sensor && !sensor.HasLock)) return;

        // Desired direction: LOS vector
        Vector3 losDir = (target.position - transform.position).normalized;
        if (losDir.sqrMagnitude < 1e-6f) return;  // already at target

        // Quaternion that rotates current forward to LOS
        Quaternion q = Quaternion.FromToRotation(transform.forward, losDir);
        q.ToAngleAxis(out float angleDeg, out Vector3 axisWorld);

        if (angleDeg < 0.01f || axisWorld == Vector3.zero) return; // no significant error

        float angleRad = angleDeg * Mathf.Deg2Rad;

        // Desired angular velocity (rad/s) to close that error in timeToAlign
        Vector3 desiredRateWorld = axisWorld.normalized * (angleRad / timeToAlign);

        // Convert to body frame and hand off to PID
        Vector3 desiredRateBody = transform.InverseTransformDirection(desiredRateWorld);
        lastAccelBody = Vector3.Cross(desiredRateBody, Vector3.forward) * rb.linearVelocity.magnitude;
        pid.ApplyRateCommand(desiredRateBody);
    }
}
