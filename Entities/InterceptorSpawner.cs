using UnityEngine;
using UnityEngine.InputSystem;

[AddComponentMenu("Simulation/Entities/Interceptor Spawner")]
public sealed class InterceptorSpawner : MonoBehaviour
{
    public GameObject missilePrefab;
    public float launchImpulse = 20f;

    void Update()
    {
        var kb = Keyboard.current;
        if (kb == null) return;
        if (kb.iKey.wasPressedThisFrame) Launch();
    }

    void Launch()
{
    if (ThreatSpawner.CurrentThreat == null)
    {
        Debug.LogWarning("No active threat to intercept.");
        return;
    }

    var go = Instantiate(missilePrefab, transform.position, transform.rotation);
    var rb = go.GetComponent<Rigidbody>();
    rb.AddForce(go.transform.forward * launchImpulse, ForceMode.Impulse);

    // Set guidance target
    var seeker = go.GetComponent<SeekerSensor>();
    if (seeker) seeker.target = ThreatSpawner.CurrentThreat;

    var guidance = go.GetComponent<GuidanceProNav>();
    if (guidance) guidance.target = ThreatSpawner.CurrentThreat;

    // 🔁 Pass missile and target to the HUD controller
    var hud = FindObjectOfType<MissileHUDController>();
    if (hud) hud.AttachMissile(go, ThreatSpawner.CurrentThreat);
}

}
