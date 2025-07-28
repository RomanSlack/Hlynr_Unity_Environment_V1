using UnityEngine;

/// <summary>Handles body-axis torques for pitch, yaw, roll.</summary>
[AddComponentMenu("Simulation/Missile/6DOF Controller")]
[RequireComponent(typeof(Rigidbody))]
public sealed class Missile6DOFController : MonoBehaviour
{
    [Tooltip("Max rotational moment (N·m) about each body axis")]
    public Vector3 maxTorque = new Vector3(8_000f, 8_000f, 2_000f);

    Rigidbody rb;

    void Awake() => rb = GetComponent<Rigidbody>();

    /// <summary>Command body-axis torques in [-1, 1] range.</summary>
    public void ApplyMoment(Vector3 demand)
    {
        demand = Vector3.Scale(demand, maxTorque);
        rb.AddRelativeTorque(demand, ForceMode.Force);       // 6DOF control :contentReference[oaicite:2]{index=2}
    }
}
