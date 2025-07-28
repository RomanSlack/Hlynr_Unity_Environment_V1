using UnityEngine;
using UnityEngine.InputSystem;  // ← add this

public class MissileLauncher : MonoBehaviour
{
    [Tooltip("Assign your Missile prefab here")]
    public GameObject missilePrefab;
    [Tooltip("Impulse applied on launch")]
    public float launchImpulse = 1000f;

    void Update()
    {
        var kb = Keyboard.current;
        if (kb == null) return;

        // Press L to fire
        if (kb.lKey.wasPressedThisFrame)
            Fire();
    }

    void Fire()
    {
        var go = Instantiate(missilePrefab, transform.position, transform.rotation);
        var rb = go.GetComponent<Rigidbody>();
        rb.AddForce(go.transform.forward * launchImpulse, ForceMode.Impulse);
    }
}
