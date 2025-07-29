using UnityEngine;

[AddComponentMenu("Simulation/Missile/Thrust Model")]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(FuelSystem))]
public sealed class ThrustModel : MonoBehaviour
{
    [Tooltip("Engine thrust profile")]
    [SerializeField] ThrustCurve curve;
    [Tooltip("Align thrust with +Z (instead of +X)")]
    [SerializeField] bool useForwardAxis = true;
    public float EvaluatedThrustN { get; private set; }  // add below fields
    Rigidbody rb;
    FuelSystem fuel;
    float burnTime;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        fuel = GetComponent<FuelSystem>();
        if (!curve) Debug.LogWarning($"{name} has no thrust curve assigned.");
    }

    void FixedUpdate()
    {
        if (fuel.IsEmpty || curve == null) return;

        float dt = Time.fixedDeltaTime;
        float thrustN = curve.Evaluate(burnTime);
        EvaluatedThrustN = thrustN;
        float massUsed = fuel.Consume(dt);

        // Apply force through centre-of-mass
        Vector3 axis = useForwardAxis ? transform.forward : transform.right;
        rb.AddForce(axis * thrustN, ForceMode.Force);        // continuous force :contentReference[oaicite:1]{index=1}

        // Reduce mass in real time (optional realism)
        rb.mass -= massUsed;

        burnTime += dt;
    }
}
