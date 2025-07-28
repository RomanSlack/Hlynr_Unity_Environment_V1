using UnityEngine;

/// Destroys both objects and triggers FX when within trigger range.
[AddComponentMenu("Simulation/Missile/Proximity Fuze")]
public sealed class ProximityFuze : MonoBehaviour
{
    [Tooltip("Explosion effect prefab")]
    public GameObject explosionPrefab;

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Threat")) return;

        Vector3 pos = transform.position;

        if (explosionPrefab)
            Instantiate(explosionPrefab, pos, Quaternion.identity);

        Destroy(other.gameObject); // Destroy threat
        Destroy(gameObject);       // Destroy self
    }
}
