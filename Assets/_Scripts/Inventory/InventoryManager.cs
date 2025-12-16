// InventoryManager.cs
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager instance;

    [SerializeField] Player player;
    [SerializeField] GameObject inventoryPanel;
    [Space]
    [SerializeField] List<Weapon> mainWeaponList;

    private bool inventoryEnable = false;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }

    private void Start()
    {
        AssignInputEvents();
    }

    void InventoryEnable()
    {
        inventoryEnable = !inventoryEnable;
        inventoryPanel.SetActive(inventoryEnable);
    }

    public void SetMainWeaponList(List<Weapon> weaponList)
    {
        mainWeaponList = new List<Weapon>(weaponList);
    }

    public List<Weapon> GetMainSlotWeaponList() => mainWeaponList;

    void AssignInputEvents()
    {
        player.controls.Inventory.Open.performed += ctx => InventoryEnable();
    }
}
