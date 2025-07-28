using UnityEngine;
using UnityEngine.InputSystem;

[AddComponentMenu("Camera Control/Fly Camera (Input System)")]
public class FlyCamera : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed   = 10f;
    public float climbSpeed  = 4f;

    [Header("Look")]
    public float lookSpeed   = 0.1f;   // degrees per pixel
    public bool  invertY     = false;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible   = false;
    }

    void Update()
    {
        var kb   = Keyboard.current;
        var ms   = Mouse.current;
        Vector3 dir = Vector3.zero;

        // — Move on WASD —
        if (kb.wKey.isPressed) dir += transform.forward;
        if (kb.sKey.isPressed) dir -= transform.forward;
        if (kb.aKey.isPressed) dir -= transform.right;
        if (kb.dKey.isPressed) dir += transform.right;

        // — Climb/Descend on E/Q —
        if (kb.eKey.isPressed) dir += transform.up * climbSpeed;
        if (kb.qKey.isPressed) dir -= transform.up * climbSpeed;

        transform.position += dir * moveSpeed * Time.deltaTime;

        // — Mouse look —
        Vector2 delta = ms.delta.ReadValue();
        float yaw   = delta.x * lookSpeed;
        float pitch = delta.y * lookSpeed * (invertY ? 1 : -1);
        transform.Rotate(-pitch, yaw, 0f, Space.Self);

        // — Unlock cursor —
        if (kb.escapeKey.wasPressedThisFrame)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible   = true;
        }
    }
}
