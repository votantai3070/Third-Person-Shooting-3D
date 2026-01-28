using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_SelectedWeaponWindow : MonoBehaviour
{
    public Weapon_Data weaponData;

    [SerializeField] private Image weaponIcon;
    [SerializeField] private TextMeshProUGUI weaponInfo;


    private void Start()
    {
        UpdateSlotInfo(null);
    }



    public void SetWeaponData(Weapon_Data newWeaponData)
    {
        weaponData = newWeaponData;
        UpdateSlotInfo(weaponData);
    }

    public void UpdateSlotInfo(Weapon_Data weaponData)
    {
        if (weaponData == null)
        {
            weaponIcon.color = Color.clear;
            weaponInfo.text = "No Weapon Selected";
            return;
        }

        weaponIcon.color = Color.white;
        weaponIcon.sprite = weaponData.weaponIcon;
        weaponInfo.text = weaponData.weaponInfo;
    }

    public bool IsEmpty() => weaponData == null;



}
