using UnityEngine;

public class PickupWeapon : MonoBehaviour
{
    [SerializeField] Weapon_SO weaponData;
    public Weapon weapon;

    [SerializeField] BackupWeaponModel[] backupWeaponModels;

    bool isOldWeapon;

    private void Start()
    {
        if (!isOldWeapon)
            weapon = new Weapon(weaponData);

        SetupGameObject();
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
