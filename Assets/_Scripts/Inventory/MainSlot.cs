using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainSlot : MonoBehaviour
{
    [SerializeField] InventoryManager inventoryManager;


    private void Update()
    {
        UpdateInvenrotyMainSlot();
    }

    private void UpdateInvenrotyMainSlot()
    {
        for (int i = 0; i < inventoryManager.GetMainSlotWeaponList().Count; i++)
        {
            Weapon weapon = inventoryManager.GetMainSlotWeaponList()[i];

            Sprite weaponIcon = weapon.weaponData.weaponIcon;
            string nameWeapon = weapon.weaponData.weaponName;

            Transform slotImageTransform = transform.GetChild(i).GetChild(0);
            Transform slotTextTransform = transform.GetChild(i).GetChild(1);
            Image iconImage = slotImageTransform.GetComponent<Image>();
            TextMeshProUGUI textNameWeapon = slotTextTransform.GetComponent<TextMeshProUGUI>();
            iconImage.sprite = weaponIcon;
            textNameWeapon.text = nameWeapon;

            // Enable image nếu có weapon, disable nếu null
            iconImage.enabled = (weaponIcon != null);
        }
    }
}
