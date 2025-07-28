using UnityEngine;

[AddComponentMenu("Simulation/Missile/Proximity Fuze")]
[RequireComponent(typeof(SphereCollider))]
public sealed class ProximityFuze : MonoBehaviour
{
    public float blastRadius = 5f;
    public GameObject explosionPrefab;
    public GameObject uiPrefab;
    public LayerMask damageMask = 1 << 8;   // example “Threat” layer

    void Reset()  // auto‑configure collider
    {
        var c = GetComponent<SphereCollider>();
        c.isTrigger = true;
        c.radius = blastRadius;
    }

    void OnTriggerEnter(Collider other)
    {
        if (((1 << other.gameObject.layer) & damageMask) == 0) return;

        Explode();
        Destroy(other.gameObject);
        Destroy(gameObject);
    }

    void Explode()
    {
        if (explosionPrefab)
            Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        // future: Physics.OverlapSphere for fragment damage

        if (uiPrefab)
            Instantiate(uiPrefab, Vector3.zero, Quaternion.identity);  // UI lives in global space
    }
}
