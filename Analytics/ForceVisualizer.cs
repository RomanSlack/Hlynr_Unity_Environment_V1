using UnityEngine;

/// Draws thrust and guidance vectors in world space for quick debugging.
/// Requires a GuidanceProNav on the same object.
[AddComponentMenu("Simulation/Analytics/Force Visualizer")]
[RequireComponent(typeof(ThrustModel))]
[RequireComponent(typeof(GuidanceProNav))]
public sealed class ForceVisualizer : MonoBehaviour
{
    public float scale = 0.2f;   // metres per newton

    ThrustModel     thrust;
    GuidanceProNav  guidance;
    LineRenderer    thrustLine;
    LineRenderer    accelLine;


    void Awake()
{
    thrust     = GetComponent<ThrustModel>();
    guidance   = GetComponent<GuidanceProNav>();

    // Only create line renderers if they don't exist already
    if (!transform.Find("ThrustLine"))
        thrustLine = CreateLine("ThrustLine", Color.blue);

    if (!transform.Find("AccelLine"))
        accelLine = CreateLine("AccelLine", Color.green);
}

    void LateUpdate()
    {
        if (!thrustLine || !accelLine) return;

        Vector3 origin = transform.position;

        // Thrust vector
        float magN = thrust.EvaluatedThrustN;
        Vector3 thrustVec = transform.forward * (magN * scale);
        Draw(thrustLine, origin, origin + thrustVec);

        // Desired acceleration (guidance lateral demand = LOS rate × navGain)
        Vector3 accBody = guidance.GetDesiredAccelBody();
        Vector3 accWorld = transform.TransformDirection(accBody) * scale;
        Draw(accelLine, origin, origin + accWorld);
    }

    LineRenderer CreateLine(string name, Color c)
    {
        var child = new GameObject(name);
        child.transform.SetParent(transform);
        var lr = child.AddComponent<LineRenderer>();
        lr.startWidth = lr.endWidth = 0.05f;
        lr.material   = new Material(Shader.Find("Sprites/Default")) { color = c };
        lr.positionCount = 2;
        return lr;
    }

    static void Draw(LineRenderer lr, Vector3 a, Vector3 b)
    {
        lr.SetPosition(0, a);
        lr.SetPosition(1, b);
    }
}
