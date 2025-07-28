using UnityEngine;
using System.Collections.Generic;

[AddComponentMenu("Simulation/Entities/Threat Spawner")]
public sealed class ThreatSpawner : MonoBehaviour
{
    public GameObject threatPrefab;
    public float spawnInterval = 10f;

    [Header("Spawn Line")]
    public Vector3 pointA = new Vector3(-100, 5, 150);
    public Vector3 pointB = new Vector3( 100, 5, 150);

    float timer;

    // Keep reference to last‑spawned threat for interceptor use
    public static Transform CurrentThreat { get; private set; }

    void FixedUpdate()
    {
        timer += Time.fixedDeltaTime;
        if (timer < spawnInterval) return;
        timer = 0f;

        Vector3 pos = Vector3.Lerp(pointA, pointB, Random.value);
        var go = Instantiate(threatPrefab, pos, Quaternion.identity);
        CurrentThreat = go.transform;
    }
}
