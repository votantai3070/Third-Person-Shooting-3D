using UnityEngine;
using UnityEngine.UI;

public class MainSlot : MonoBehaviour
{
    [SerializeField] InventoryManager inventoryManager;

    private void Start()
    {
        Debug.Log("inventoryManager.GetMainSlotWeaponList().Count: " + inventoryManager.GetMainSlotWeaponList().Count);
    }

    private void Update()
    {
        for (int i = 0; i < inventoryManager.GetMainSlotWeaponList().Count; i++)
        {
            Weapon weapon = inventoryManager.GetMainSlotWeaponList()[i];

            Sprite weaponIcon = weapon.weaponData.weaponIcon;

            Transform slotTransform = transform.GetChild(i).GetChild(0);
            Image iconImage = slotTransform.GetComponent<Image>();
            iconImage.sprite = weaponIcon;

            // Enable image nếu có weapon, disable nếu null
            iconImage.enabled = (weaponIcon != null);
        }

    }
}
