using System.Collections.Generic;
using UnityEngine;

public enum AmmoBoxType
{
    smallBox, bigBox
}

[System.Serializable]
public struct AmmoData
{
    public WeaponType weaponType;
    [UnityEngine.Range(10, 100)] public int amount;
}

public class PickupAmmo : Interactable
{
    [SerializeField] AmmoBoxType ammoBoxType;

    [SerializeField] List<AmmoData> smallBoxAmmo;
    [SerializeField] List<AmmoData> bigBoxAmmo;

    [SerializeField] private GameObject[] boxModel;

    private void Start()
    {
        SetupBoxModel();
    }

    public override void Interact()
    {
        base.Interact();


        List<AmmoData> currentAmmoList = smallBoxAmmo;

        if (ammoBoxType == AmmoBoxType.bigBox)
            currentAmmoList = bigBoxAmmo;

        foreach (AmmoData ammo in currentAmmoList)
        {
            AmmoInventoryUI.instance.AddAmmo(ammo.weaponType, ammo.amount);
        }

        ObjectPool.instance.ReturnToPool(gameObject);
    }


    private void SetupBoxModel()
    {
        for (int i = 0; i < boxModel.Length; i++)
        {
            boxModel[i].SetActive(false);


            if (i == ((int)ammoBoxType))
            {
                boxModel[i].SetActive(true);
            }
        }
    }
}
