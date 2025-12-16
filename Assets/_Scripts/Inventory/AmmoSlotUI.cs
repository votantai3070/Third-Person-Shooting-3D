using UnityEngine;

public class AmmoSlotUI : MonoBehaviour
{
    [SerializeField] Ammo ammo;

    public void SetAmmo(Ammo ammo)
    {
        this.ammo = ammo;
    }

    public void AddAmmoAmount(int amount) => ammo.AddAmmo(amount);

    public void MinusAmmoAmount(int amount) => ammo.MinusAmmo(amount);

    public Ammo GetAmmo() => ammo;
}
