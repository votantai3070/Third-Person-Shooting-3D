using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AmmoInventoryUI : MonoBehaviour
{
    public static AmmoInventoryUI instance;

    [SerializeField] Sprite[] sprites;
    [SerializeField] GameObject ammoSlotPrefab;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        GenerateAmmoSlot();
        GenerateAmmoSlotInfo();
    }

    private void Update()
    {
        UpdateAllAmmoAmounts();
    }

    private void GenerateAmmoSlot()
    {
        for (int i = 0; i < sprites.Length; i++)
        {
            GameObject ammoSlot = Instantiate(ammoSlotPrefab, transform);
            ammoSlot.AddComponent<AmmoSlotUI>();
        }
    }

    private void GenerateAmmoSlotInfo()
    {
        for (int i = 0; i < sprites.Length; i++)
        {
            AmmoType ammoType = ParseAmmoType(sprites[i].name);
            WeaponType weaponType = GetCompatibleWeapon(ammoType);

            Ammo ammo = new(0, sprites[i].name, ammoType, weaponType);

            Transform slotTransform = transform.GetChild(i);
            Image ammoImage = slotTransform.GetComponent<Image>();
            AmmoSlotUI ammoTypeWeapon = ammoImage.GetComponent<AmmoSlotUI>();

            // Setup data
            ammoTypeWeapon.SetAmmo(ammo);
            ammoImage.sprite = sprites[i];
        }

        UpdateAllAmmoAmounts();
    }

    private void UpdateAllAmmoAmounts()
    {
        AmmoSlotUI[] ammoTypeWeapons = GetComponentsInChildren<AmmoSlotUI>(true);

        foreach (var ammoTypeWeapon in ammoTypeWeapons)
        {
            TextMeshProUGUI ammoAmount = ammoTypeWeapon.transform.GetChild(0).GetComponent<TextMeshProUGUI>();

            int totalAmmo = ammoTypeWeapon.GetAmmo().GetTotalAmmo();
            ammoAmount.text = totalAmmo.ToString();
        }
    }

    public void AddAmmo(WeaponType weaponType, int amount)
    {
        AmmoSlotUI[] ammoTypeWeapons = GetComponentsInChildren<AmmoSlotUI>(true);

        foreach (var ammoTypeWeapon in ammoTypeWeapons)
        {
            if (ammoTypeWeapon.GetAmmo().weaponType == weaponType)
            {
                ammoTypeWeapon.AddAmmoAmount(amount);
            }
        }
    }

    public void MinusAmmo(Weapon_SO weaponData, int amount)
    {
        AmmoSlotUI[] ammoTypeWeapons = GetComponentsInChildren<AmmoSlotUI>(true);

        foreach (var ammoTypeWeapon in ammoTypeWeapons)
        {
            if (ammoTypeWeapon.GetAmmo().weaponType == weaponData.weaponType)
            {
                ammoTypeWeapon.MinusAmmoAmount(amount);
            }
        }
    }


    private AmmoType ParseAmmoType(string spriteName)
    {
        if (spriteName.Contains("7.62x39"))
            return AmmoType._7_62x39mm;

        if (spriteName.Contains("7.62x25") || spriteName.Contains("762x25"))
            return AmmoType._7_62x25mm;

        if (spriteName.Contains("9.19") || spriteName.Contains("9mm"))
            return AmmoType._9_19mm;

        if (spriteName.Contains("5.56") || spriteName.Contains("556"))
            return AmmoType._5_56mm;

        if (spriteName.Contains("Snip"))
            return AmmoType.snip;

        if (spriteName.Contains("Shotgun"))
            return AmmoType.shotgun;

        // Default
        return AmmoType._7_62x25mm;
    }


    private WeaponType GetCompatibleWeapon(AmmoType ammoType)
    {
        return ammoType switch
        {
            AmmoType._7_62x25mm => WeaponType.M1991,     // Pistol
            AmmoType._9_19mm => WeaponType.Uzi,          // SMG
            AmmoType._5_56mm => WeaponType.M4,           // Rifle
            AmmoType._7_62x39mm => WeaponType.AK74,      // AK Rifle
            AmmoType.snip => WeaponType.M107,            // Sniper
            AmmoType.shotgun => WeaponType.Bennel_M4,    // Shotgun
            _ => WeaponType.M1991                        // Default
        };
    }
}
