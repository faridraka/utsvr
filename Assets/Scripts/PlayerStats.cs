using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class VR01_P5_Latihan3 : MonoBehaviour
{
    [Header("HP")]
    public int MaxHP = 100;
    public int CurrentHP;
    public TextMeshProUGUI TextHP;

    [Header("Ammo")]
    public int MaxAmmo = 15;
    public int CurrentAmmo;
    public TextMeshProUGUI TextAmmo;

    [Header("Buttons")]
    public Button ButtonShoot;
    public Button ButtonReload;
    public Button ButtonDamage;
    public Button ButtonHeal;

    // Start is called once before the first execution of Update
    void Start()
    {
        // Inisialisasi HP dan Ammo
        CurrentHP = MaxHP;
        CurrentAmmo = MaxAmmo;

        UpdateUI();

        // Hubungkan tombol dengan fungsi
        ButtonShoot.onClick.AddListener(Shoot);
        ButtonReload.onClick.AddListener(Reload);
        ButtonDamage.onClick.AddListener(Damage);
        ButtonHeal.onClick.AddListener(Heal);
    }

    void UpdateUI()
    {
        // Update tulisan
        TextHP.text = "HP " + CurrentHP + "/" + MaxHP;
        TextAmmo.text = "Ammo " + CurrentAmmo + "/" + MaxAmmo;

        // Warna HP
        if (CurrentHP <= 30)
        {
            TextHP.color = Color.red;
        }
        else
        {
            TextHP.color = Color.green;
        }

        // Warna Ammo
        if (CurrentAmmo <= 5)
        {
            TextAmmo.color = Color.yellow;
        }
        else
        {
            TextAmmo.color = Color.white;
        }

        // Disable Shoot kalau ammo habis
        ButtonShoot.interactable = CurrentAmmo > 0;
    }

    public void Shoot()
    {
        if (CurrentAmmo > 0)
        {
            CurrentAmmo--;
            UpdateUI();
        }
    }

    public void Reload()
    {
        CurrentAmmo = MaxAmmo;
        UpdateUI();
    }

    public void Damage()
    {
        CurrentHP -= 10;

        if (CurrentHP < 0)
        {
            CurrentHP = 0;
        }

        UpdateUI();
    }

    public void Heal()
    {
        CurrentHP += 10;

        if (CurrentHP > MaxHP)
        {
            CurrentHP = MaxHP;
        }

        UpdateUI();
    }
}