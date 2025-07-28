using UnityEngine;

[CreateAssetMenu(menuName = "Simulation/Missile/Thrust Curve")]
public sealed class ThrustCurve : ScriptableObject
{
    [Tooltip("Time-indexed thrust samples (seconds, newtons)")]
    public AnimationCurve thrustNewton = AnimationCurve.Linear(0f, 600f, 2f, 0f);

    public float Evaluate(float t) => thrustNewton.Evaluate(t);
}
