using System;
using UnityEngine;

public enum AmmoType
{
    _7_62x25mm, _5_56mm, _7_62x39mm, _9_19mm, snip, shotgun
}

[Serializable]
public class Ammo
{
    public WeaponType weaponType;
    public AmmoType ammoType;

    [SerializeField] string ammoName;
    [SerializeField] int totalAmmo;

    public Ammo(int totalAmmo, string ammoName, AmmoType ammoType, WeaponType weaponType)
    {
        this.ammoType = ammoType;
        this.totalAmmo = totalAmmo;
        this.ammoName = ammoName;
        this.weaponType = weaponType;
    }

    public void AddAmmo(int amount) => totalAmmo += amount;

    public void MinusAmmo(int amount) => totalAmmo -= amount;

    public int GetTotalAmmo() => totalAmmo;
    public string GetAmmoName() => ammoName;
}
