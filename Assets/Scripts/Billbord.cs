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

    Vector3 dir = targetCamera.transform.position - transform.position;
    Quaternion lookRot = Quaternion.LookRotation(dir);

    Vector3 euler = lookRot.eulerAngles;

    transform.rotation = Quaternion.Euler(
        0f,         // X dikunci
        euler.y,    // hanya Y yang mengikuti
        0f          // Z dikunci
    );
}
}
