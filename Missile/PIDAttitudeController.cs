using UnityEngine;
using UnityEngine.InputSystem;   // only here for later hot‑tuning if desired

/// Closed‑loop body‑rate controller (rad/s → torque)
[AddComponentMenu("Simulation/Missile/PID Attitude Controller")]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Missile6DOFController))]
public sealed class PIDAttitudeController : MonoBehaviour
{
    public Vector3 Kp = new Vector3(0.8f, 0.8f, 0.2f);
    public Vector3 Ki = Vector3.zero;
    public Vector3 Kd = new Vector3(0.05f, 0.05f, 0.02f);

    Vector3 integ;
    Vector3 prevErr;

    Rigidbody           rb;
    Missile6DOFController actuator;

    void Awake()
    {
        rb        = GetComponent<Rigidbody>();
        actuator  = GetComponent<Missile6DOFController>();
    }

    /// Call once per FixedUpdate with desired body‑rates (rad/s)
    public void ApplyRateCommand(Vector3 desiredBodyRate)
    {
        Vector3 currentRateBody = transform.InverseTransformDirection(rb.angularVelocity);
        Vector3 err            = desiredBodyRate - currentRateBody;

        integ      += err * Time.fixedDeltaTime;
        Vector3 deriv = (err - prevErr) / Time.fixedDeltaTime;
        prevErr    = err;

        Vector3 torqueCmd =
            Vector3.Scale(Kp, err) +
            Vector3.Scale(Ki, integ) +
            Vector3.Scale(Kd, deriv);

        // Normalise into ‑1…1 range for the 6DOF actuator
        Vector3 norm =
            new Vector3(
                Mathf.Clamp(torqueCmd.x / actuator.maxTorque.x, -1f, 1f),
                Mathf.Clamp(torqueCmd.y / actuator.maxTorque.y, -1f, 1f),
                Mathf.Clamp(torqueCmd.z / actuator.maxTorque.z, -1f, 1f));

        actuator.ApplyMoment(norm);
    }
}
