using System.Collections.Generic;
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
        Debug.Log("Updating Inventory Main Slot...");

        List<Weapon> weapons = inventoryManager.GetMainSlotWeaponList();
        Debug.Log($"Main Slot Weapon Count: {weapons.Count}");

        int totalSlots = transform.childCount;

        for (int i = 0; i < totalSlots; i++)
        {
            Transform slot = transform.GetChild(i);
            Image iconImage = slot.GetChild(0).GetComponent<Image>();
            TextMeshProUGUI textName = slot.GetChild(1).GetComponent<TextMeshProUGUI>();

            if (i < weapons.Count && weapons[i] != null)
            {
                Weapon weapon = weapons[i];
                iconImage.sprite = weapon.weaponData.weaponIcon;
                iconImage.enabled = true;
                textName.text = weapon.weaponData.weaponName;
            }
            else
            {
                iconImage.sprite = null;
                iconImage.enabled = false;
                textName.text = "";
            }
        }
    }

}
