using UnityEngine;

/// <summary>
/// Billboard.cs
/// Panel selalu menghadap kamera player (billboard effect)
/// Attach ke: GameObject "WristCanvas"
/// </summary>
public class Billboard : MonoBehaviour
{
    [Header("=== KAMERA TARGET ===")]
    [Tooltip("Drag PlayerCamera ke sini")]
    public Camera targetCamera;

    [Header("=== SETTING ===")]
    [Tooltip("Smooth rotation speed")]
    public float rotationSpeed = 8f;

    void Start()
    {
        // Auto-find kamera kalau belum di-assign
        if (targetCamera == null)
            targetCamera = Camera.main;
    }

    void LateUpdate()
    {
        if (targetCamera == null) return;

        // Panel menghadap arah dari panel ke kamera
        Vector3 direction = transform.position - targetCamera.transform.position;

        if (direction == Vector3.zero) return;

        Quaternion targetRotation = Quaternion.LookRotation(direction);

        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            targetRotation,
            Time.deltaTime * rotationSpeed
        );
    }
}