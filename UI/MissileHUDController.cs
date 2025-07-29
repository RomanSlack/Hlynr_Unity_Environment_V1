using UnityEngine;
using UnityEngine.UIElements;

[AddComponentMenu("Simulation/UI/Missile HUD Controller")]
public sealed class MissileHUDController : MonoBehaviour
{
    UIDocument doc;
    Label speedLbl, fuelLbl, lockLbl, missLbl;

    Missile6DOFController currentMissile;
    SeekerSensor seeker;
    Rigidbody   rb;
    FuelSystem  fuel;

    void Awake()
    {
        doc       = GetComponent<UIDocument>();
        var root  = doc.rootVisualElement;

        speedLbl  = root.Q<Label>("speed");
        fuelLbl   = root.Q<Label>("fuel");
        lockLbl   = root.Q<Label>("lock");
        missLbl   = root.Q<Label>("miss");

        Debug.Log($"Labels found? speed={speedLbl != null}, fuel={fuelLbl != null}");
    }

    public void AttachMissile(GameObject missile, Transform target)
    {
        Debug.Log($"HUD attached to missile: {missile?.name}, target: {target?.name}");
        currentMissile = missile ? missile.GetComponent<Missile6DOFController>() : null;
        rb    = missile ? missile.GetComponent<Rigidbody>()    : null;
        fuel  = missile ? missile.GetComponent<FuelSystem>()   : null;
        seeker= missile ? missile.GetComponent<SeekerSensor>() : null;

        // compute straight‑line miss distance at time of attach
        if (missLbl != null && missile != null && target != null)
        {
            float miss = Vector3.Distance(missile.transform.position, target.position);
            missLbl.text = $"Miss : {miss:0} m";
        }
    }

    void Update()
    {
        Debug.Log("HUD Update() running");
        if (currentMissile == null) return;

        speedLbl.text = $"Speed: {rb.linearVelocity.magnitude:0} m/s";
        fuelLbl .text = $"Fuel : {(fuel ? fuel.fuelKg : 0):0.0} kg";

        if (seeker && seeker.HasLock)
        {
            lockLbl.text  = "Lock : YES";
            lockLbl.style.color = new StyleColor(Color.green);
        }
        else
        {
            lockLbl.text  = "Lock : NO";
            lockLbl.style.color = new StyleColor(new Color(1f,0.3f,0.3f));
        }
    }
}
