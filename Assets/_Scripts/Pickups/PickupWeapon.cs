using UnityEngine;

public class PickupWeapon : Interactable
{
    [SerializeField] Weapon_Data weaponData;
    public Weapon weapon;

    [SerializeField] BackupWeaponModel[] backupWeaponModels;

    bool isOldWeapon;

    private void Start()
    {
        if (!isOldWeapon)
        {
            weapon = new Weapon(weaponData);
        }

        SetupGameObject();
    }

    public override void Interact()
    {
        base.Interact();

        if (!isOldWeapon)
        {
            // Súng mới: Add ammo vào inventory TRƯỚC
            AmmoInventoryUI.instance.AddAmmo(weapon.weaponType, weapon.totalReserveAmmo);
        }

        // Cả súng mới và cũ đều GET ammo từ inventory
        weapon.totalReserveAmmo = GetAmmoFromInventory(weapon.weaponType);

        Debug.Log("Weapon: " + weapon);
        Debug.Log("Player: " + player);
        player.controller.GetListWeapon().Add(weapon);
        ObjectPool.instance.DelayReturnToPool(gameObject);
    }

    // Method lấy ammo từ inventory
    private int GetAmmoFromInventory(WeaponType weaponType)
    {
        AmmoSlotUI[] ammoSlots = AmmoInventoryUI.instance.GetComponentsInChildren<AmmoSlotUI>(true);

        foreach (var ammoSlot in ammoSlots)
        {
            if (weaponType == ammoSlot.GetAmmo().weaponType)
            {
                return ammoSlot.GetAmmo().GetTotalAmmo();
            }
        }

        return 0;
    }

    public void SetupPickupWeapon(Transform transform, Weapon weapon)
    {
        isOldWeapon = true;

        this.weapon = weapon;
        weaponData = weapon.weaponData;

        this.transform.position = transform.position;
    }

    [ContextMenu("Update Weapon Model")]
    void SetupGameObject()
    {
        gameObject.name = "Pickup_Weapon - " + weaponData.weaponType.ToString();

        SetupWeaponModel();
    }

    private void SetupWeaponModel()
    {
        backupWeaponModels = GetComponentsInChildren<BackupWeaponModel>();

        foreach (var weaponModel in backupWeaponModels)
        {
            weaponModel.gameObject.SetActive(false);

            if (weaponModel.weaponType == weaponData.weaponType)
            {
                weaponModel.gameObject.SetActive(true);
            }
        }
    }
}
