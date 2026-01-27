using UnityEngine;

public class UI_WeaponSelection : MonoBehaviour
{
    public UI_SelectedWeaponWindow[] seletedWeapon;

    private void Start()
    {
        seletedWeapon = GetComponentsInChildren<UI_SelectedWeaponWindow>();
    }

    public UI_SelectedWeaponWindow FindEmptySlot()
    {
        foreach (var slot in seletedWeapon)
        {
            if (slot.IsEmpty())
                return slot;
        }
        return null;
    }

    public UI_SelectedWeaponWindow FindSlotWithWeaponOfType(Weapon_Data weaponData)
    {
        for (int i = 0; i < seletedWeapon.Length; i++)
        {
            if (seletedWeapon[i].weaponData == weaponData)
            {
                return seletedWeapon[i];
            }
        }

        return null;
    }
}
