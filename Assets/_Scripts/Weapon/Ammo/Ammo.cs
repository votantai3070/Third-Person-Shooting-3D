using UnityEngine;

public class Ammo
{
    [SerializeField] string ammoName;
    [SerializeField] int totalAmmo;

    public Ammo(int totalAmmo, string ammoName)
    {
        this.totalAmmo = totalAmmo;
        this.ammoName = ammoName;
    }

    public void AddAmmo(int amount) => totalAmmo += amount;

    public void MinusAmmo(int amount) => totalAmmo -= amount;

    public int GetTotalMinus() => totalAmmo;
    public string GetAmmoName() => ammoName;
}
