using UnityEngine;

/// <summary>
/// WristFollowPlayer.cs
/// Membuat World Space Canvas mengikuti player seperti wrist panel
/// Attach ke: GameObject "WristCanvas" (yang ada World Space Canvas)
///
/// BONUS POIN: Panel mengikuti/menempel dekat player dengan nyaman
/// </summary>
public class WristFollowPlayer : MonoBehaviour
{
    [Header("=== TARGET ===")]
    public Transform playerTransform;     // Drag Player GameObject ke sini
    public Camera playerCamera;           // Drag Player Camera ke sini

    [Header("=== POSISI PANEL ===")]
    [Tooltip("Offset posisi dari player (kiri/kanan/atas/bawah)")]
    public Vector3 positionOffset = new Vector3(0.35f, -0.25f, 0.6f);

    [Tooltip("Seberapa smooth panel mengikuti player (lerp speed)")]
    public float followSpeed = 10f;

    [Tooltip("Seberapa smooth rotasi panel mengikuti kamera")]
    public float rotationSpeed = 8f;

    [Header("=== BILLBOARD (BONUS) ===")]
    [Tooltip("Panel selalu menghadap kamera (Billboard effect)")]
    public bool useBillboard = true;

    private Vector3 targetPosition;
    private Quaternion targetRotation;

    void LateUpdate()
    {
        if (playerCamera == null || playerTransform == null) return;

        // Hitung posisi target relatif terhadap kamera player
        targetPosition = playerCamera.transform.position
                        + playerCamera.transform.right * positionOffset.x
                        + playerCamera.transform.up * positionOffset.y
                        + playerCamera.transform.forward * positionOffset.z;

        // Smooth follow posisi
        transform.position = Vector3.Lerp(
            transform.position,
            targetPosition,
            Time.deltaTime * followSpeed
        );

        // Billboard: panel selalu menghadap kamera
        if (useBillboard)
        {
            Vector3 dirToCamera = transform.position - playerCamera.transform.position;
            if (dirToCamera != Vector3.zero)
            {
                targetRotation = Quaternion.LookRotation(dirToCamera);
                transform.rotation = Quaternion.Slerp(
                    transform.rotation,
                    targetRotation,
                    Time.deltaTime * rotationSpeed
                );
            }
        }
    }
}
