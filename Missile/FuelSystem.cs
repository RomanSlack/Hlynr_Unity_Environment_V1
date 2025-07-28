using UnityEngine;

[AddComponentMenu("Simulation/Missile/Fuel System")]
public sealed class FuelSystem : MonoBehaviour
{
    [Tooltip("Initial fuel mass (kg)")] public float fuelKg = 25f;
    [Tooltip("Mass flow rate (kg/s)")] public float massFlowKgPerSec = 0.5f;

    public bool IsEmpty => fuelKg <= 0f;

    public float Consume(float dt)
    {
        if (IsEmpty) return 0f;
        float used = massFlowKgPerSec * dt;
        if (used > fuelKg) used = fuelKg;
        fuelKg -= used;
        return used;
    }
}
