using UnityEngine;
using UnityEngine.InputSystem;

public class ThirdPersonCamera : MonoBehaviour
{
    public Transform target;         // Player transform
    public Vector3 offset = new Vector3(0f, 3f, -6f);
    public float sensitivity = 3f;
    public float distance = 6f;
    public float rotationSmoothTime = 0.12f;

    private float yaw;   // horizontal angle (mouse X)
    private float pitch; // vertical angle (mouse Y)

    void LateUpdate()
    {
        if (target == null) return;

        // Mouse input
        yaw += Mouse.current.delta.x.ReadValue() * sensitivity;
        pitch -= Mouse.current.delta.y.ReadValue() * sensitivity;
        pitch = Mathf.Clamp(pitch, -30f, 60f); // prevent camera flipping

        // Calculate camera position
        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0);
        Vector3 desiredPosition = target.position + rotation * offset;

        transform.position = desiredPosition;
        transform.LookAt(target.position + Vector3.up * 1.5f); // look at player upper body
    }
}
