using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// WristPanelManager.cs
/// Script utama untuk Soal 6 - Wrist Survival Panel
/// Attach ke: GameObject "WristPanel" (World Space Canvas)
/// </summary>
public class WristPanelManager : MonoBehaviour
{
    [Header("=== STAT VALUES ===")]
    public int maxHP = 100;
    public int currentHP = 100;
    public int maxAmmo = 15;
    public int currentAmmo = 15;
    public int damageAmount = 20;
    public int healAmount = 20;

    [Header("=== HP UI ===")]
    public TextMeshProUGUI hpText;           // Text "100/100"
    public Image hpBarFill;                  // Image fillAmount untuk bar HP
    public Color hpColorNormal = Color.green;
    public Color hpColorLow = Color.red;
    public int hpLowThreshold = 30;

    [Header("=== AMMO UI ===")]
    public TextMeshProUGUI ammoText;         // Text "15/15"
    public Image ammoBarFill;               // Image fillAmount untuk bar Ammo (reload)
    public Color ammoColorNormal = Color.cyan;
    public Color ammoColorLow = new Color(1f, 0.5f, 0f); // orange
    public int ammoLowThreshold = 5;

    [Header("=== BUTTONS ===")]
    public Button shootButton;
    public Button reloadButton;
    public Button damageButton;
    public Button healButton;

    [Header("=== EFFECT OVERLAY ===")]
    public Image damageOverlay;    // Image full-screen merah transparan
    public Image healOverlay;      // Image full-screen hijau transparan
    public float overlayDuration = 0.4f;

    [Header("=== STATUS TEXT ===")]
    public TextMeshProUGUI statusText;       // "Shooting...", "Reloading...", etc
    public TextMeshProUGUI gameOverText;     // "YOU DIED"

    [Header("=== RELOAD SETTINGS ===")]
    public float reloadDuration = 2f;

    // --- Private state ---
    private bool isReloading = false;
    private bool isDead = false;

    void Start()
    {
        // Inisialisasi nilai awal
        currentHP = maxHP;
        currentAmmo = maxAmmo;

        // Sembunyikan overlay dan game over
        SetOverlayAlpha(damageOverlay, 0f);
        SetOverlayAlpha(healOverlay, 0f);
        if (gameOverText != null) gameOverText.gameObject.SetActive(false);
        if (statusText != null) statusText.text = "";

        // Reset reload bar
        if (ammoBarFill != null) ammoBarFill.fillAmount = 1f;

        UpdateAllUI();
    }

    // ============================================================
    //  BUTTON FUNCTIONS
    // ============================================================

    /// <summary>Dipanggil oleh Button SHOOT</summary>
    public void OnShoot()
    {
        if (isDead || isReloading) return;
        if (currentAmmo <= 0)
        {
            ShowStatus("No ammo! Reload first.");
            return;
        }

        currentAmmo--;
        ShowStatus("Shoot!");
        UpdateAllUI();

        if (currentAmmo <= 0)
        {
            shootButton.interactable = false;
            ShowStatus("Out of ammo!");
        }
    }

    /// <summary>Dipanggil oleh Button RELOAD</summary>
    public void OnReload()
    {
        if (isDead || isReloading) return;
        if (currentAmmo == maxAmmo)
        {
            ShowStatus("Already full ammo.");
            return;
        }

        StartCoroutine(ReloadCoroutine());
    }

    /// <summary>Dipanggil oleh Button DAMAGE (untuk demo/testing)</summary>
    public void OnDamage()
    {
        if (isDead) return;

        currentHP -= damageAmount;
        currentHP = Mathf.Max(0, currentHP);

        StartCoroutine(FlashOverlay(damageOverlay));
        UpdateAllUI();

        if (currentHP <= 0)
        {
            TriggerDeath();
        }
        else
        {
            ShowStatus("Ouch! Took " + damageAmount + " damage.");
        }
    }

    /// <summary>Dipanggil oleh Button HEAL (untuk demo/testing)</summary>
    public void OnHeal()
    {
        if (isDead) return;
        if (currentHP >= maxHP)
        {
            ShowStatus("HP already full!");
            return;
        }

        currentHP += healAmount;
        currentHP = Mathf.Min(maxHP, currentHP);

        StartCoroutine(FlashOverlay(healOverlay));
        UpdateAllUI();
        ShowStatus("Healed +" + healAmount + " HP.");
    }

    // ============================================================
    //  COROUTINES
    // ============================================================

    private IEnumerator ReloadCoroutine()
    {
        isReloading = true;
        SetButtonsInteractable(shoot: false, reload: false);
        ShowStatus("Reloading...");

        float elapsed = 0f;
        int startAmmo = currentAmmo;

        // Reset bar ke posisi saat ini
        if (ammoBarFill != null)
            ammoBarFill.fillAmount = (float)startAmmo / maxAmmo;

        while (elapsed < reloadDuration)
        {
            elapsed += Time.deltaTime;
            float progress = elapsed / reloadDuration;

            // Animasi fillAmount dari current ke penuh
            if (ammoBarFill != null)
                ammoBarFill.fillAmount = Mathf.Lerp((float)startAmmo / maxAmmo, 1f, progress);

            // Update text ammo secara live selama reload
            int displayAmmo = Mathf.RoundToInt(Mathf.Lerp(startAmmo, maxAmmo, progress));
            if (ammoText != null)
                ammoText.text = displayAmmo + "/" + maxAmmo;

            yield return null;
        }

        currentAmmo = maxAmmo;
        isReloading = false;
        UpdateAllUI();
        ShowStatus("Reload complete!");
        SetButtonsInteractable(shoot: true, reload: true);
    }

    private IEnumerator FlashOverlay(Image overlay)
    {
        if (overlay == null) yield break;

        float halfTime = overlayDuration / 2f;

        // Fade in
        float t = 0f;
        while (t < halfTime)
        {
            t += Time.deltaTime;
            SetOverlayAlpha(overlay, Mathf.Lerp(0f, 0.5f, t / halfTime));
            yield return null;
        }

        // Fade out
        t = 0f;
        while (t < halfTime)
        {
            t += Time.deltaTime;
            SetOverlayAlpha(overlay, Mathf.Lerp(0.5f, 0f, t / halfTime));
            yield return null;
        }

        SetOverlayAlpha(overlay, 0f);
    }

    // ============================================================
    //  UI UPDATE FUNCTIONS
    // ============================================================

    private void UpdateAllUI()
    {
        UpdateHPUI();
        UpdateAmmoUI();
        UpdateButtonStates();
    }

    private void UpdateHPUI()
    {
        if (hpText != null)
        {
            hpText.text = currentHP + "/" + maxHP;
            hpText.color = (currentHP <= hpLowThreshold) ? hpColorLow : hpColorNormal;
        }

        if (hpBarFill != null)
        {
            hpBarFill.fillAmount = (float)currentHP / maxHP;
            hpBarFill.color = (currentHP <= hpLowThreshold) ? hpColorLow : hpColorNormal;
        }
    }

    private void UpdateAmmoUI()
    {
        if (ammoText != null)
        {
            ammoText.text = currentAmmo + "/" + maxAmmo;
            ammoText.color = (currentAmmo <= ammoLowThreshold) ? ammoColorLow : ammoColorNormal;
        }

        if (ammoBarFill != null && !isReloading)
        {
            ammoBarFill.fillAmount = (float)currentAmmo / maxAmmo;
            ammoBarFill.color = (currentAmmo <= ammoLowThreshold) ? ammoColorLow : ammoColorNormal;
        }
    }

    private void UpdateButtonStates()
    {
        if (isDead || isReloading) return;

        if (shootButton != null)
            shootButton.interactable = currentAmmo > 0;

        if (reloadButton != null)
            reloadButton.interactable = currentAmmo < maxAmmo;
    }

    private void SetButtonsInteractable(bool shoot, bool reload)
    {
        if (shootButton != null) shootButton.interactable = shoot;
        if (reloadButton != null) reloadButton.interactable = reload;
        if (damageButton != null) damageButton.interactable = !isReloading;
        if (healButton != null) healButton.interactable = !isReloading;
    }

    private void ShowStatus(string message)
    {
        if (statusText != null)
            statusText.text = message;
    }

    private void SetOverlayAlpha(Image img, float alpha)
    {
        if (img == null) return;
        Color c = img.color;
        c.a = alpha;
        img.color = c;
    }

    private void TriggerDeath()
    {
        isDead = true;
        ShowStatus("");

        if (gameOverText != null)
            gameOverText.gameObject.SetActive(true);

        // Disable semua button
        if (shootButton != null) shootButton.interactable = false;
        if (reloadButton != null) reloadButton.interactable = false;
        if (damageButton != null) damageButton.interactable = false;
        if (healButton != null) healButton.interactable = false;

        // Respawn setelah 3 detik (opsional)
        StartCoroutine(RespawnAfterDelay(3f));
    }

    private IEnumerator RespawnAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Respawn();
    }

    /// <summary>Reset semua stat ke nilai awal</summary>
    public void Respawn()
    {
        isDead = false;
        currentHP = maxHP;
        currentAmmo = maxAmmo;

        if (gameOverText != null) gameOverText.gameObject.SetActive(false);
        ShowStatus("Respawned!");

        UpdateAllUI();
        SetButtonsInteractable(shoot: true, reload: false);
    }
}
