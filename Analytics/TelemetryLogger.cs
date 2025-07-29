using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Text;

[AddComponentMenu("Simulation/Analytics/Telemetry Logger")]
[RequireComponent(typeof(Rigidbody))]
public sealed class TelemetryLogger : MonoBehaviour
{
    [Tooltip("Optional custom file name; leave blank for auto.")]
    public string fileStem = "";

    [Tooltip("Optional absolute folder path. If blank, uses persistentDataPath.")]
    public string outputDirectory = "";

    [Tooltip("Write to disk every N seconds; 0 = only on Destroy")]
    public float flushInterval = 2f;

    Rigidbody rb;
    FuelSystem fuel;
    ThrustModel thrust;
    StreamWriter writer;
    float flushTimer;

    readonly StringBuilder sb = new StringBuilder(256);

    void Awake()
    {
        rb     = GetComponent<Rigidbody>();
        fuel   = GetComponent<FuelSystem>();
        thrust = GetComponent<ThrustModel>();

        string stem = string.IsNullOrWhiteSpace(fileStem)
            ? $"missile_{System.DateTime.Now:yyyyMMdd_HHmmss_fff}"
            : fileStem;

        string dir = string.IsNullOrWhiteSpace(outputDirectory)
            ? Application.persistentDataPath
            : outputDirectory;

        try
        {
            Directory.CreateDirectory(dir);  // safe even if it already exists
        }
        catch (IOException ex)
        {
            Debug.LogError($"TelemetryLogger: Failed to create directory '{dir}': {ex.Message}");
            dir = Application.persistentDataPath;  // fallback
        }

        string path = Path.Combine(dir, stem + ".csv");

        try
        {
            writer = new StreamWriter(path, false, Encoding.UTF8);
            writer.WriteLine("time,pos.x,pos.y,pos.z,vel.x,vel.y,vel.z,fuel_kg,thrust_N");
        }
        catch (IOException ex)
        {
            Debug.LogError($"TelemetryLogger: Failed to open file '{path}': {ex.Message}");
            this.enabled = false;
        }
    }

    void FixedUpdate()
    {
        if (writer == null) return;

        var p = rb.position;
        var v = rb.linearVelocity;

        sb.Clear();
        sb.Append(Time.time).Append(',')
          .Append(p.x).Append(',').Append(p.y).Append(',').Append(p.z).Append(',')
          .Append(v.x).Append(',').Append(v.y).Append(',').Append(v.z).Append(',')
          .Append(fuel ? fuel.fuelKg : 0f).Append(',')
          .Append(thrust ? thrust.EvaluatedThrustN : 0f);

        writer.WriteLine(sb.ToString());

        if (flushInterval > 0f)
        {
            flushTimer += Time.fixedDeltaTime;
            if (flushTimer >= flushInterval)
            {
                writer.Flush();
                flushTimer = 0f;
            }
        }
    }

    void OnDestroy()
    {
        if (writer == null) return;
        writer.Flush();
        writer.Dispose();
        Debug.Log($"Telemetry saved to {writer.BaseStream}");
    }
}
