using UnityEngine;

/// Simple straight‑line threat: constant velocity toward an aim point.
[AddComponentMenu("Simulation/Entities/Threat Guidance")]
[RequireComponent(typeof(Rigidbody))]
public sealed class ThreatGuidance : MonoBehaviour
{
    public Vector3 aimPoint = Vector3.zero;
    public float velocity = 50f;

    Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        rb.isKinematic = true;          // we control position manually
    }

    void FixedUpdate()
    {
        Vector3 dir = (aimPoint - transform.position).normalized;
        rb.MovePosition(transform.position + dir * velocity * Time.fixedDeltaTime);
        transform.rotation = Quaternion.LookRotation(dir);
    }
}
