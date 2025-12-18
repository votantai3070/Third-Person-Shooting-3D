// InventoryManager.cs
using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager instance;

    [SerializeField] Player player;
    [SerializeField] GameObject inventoryPanel;
    [Space]
    [SerializeField] List<Weapon> mainWeaponList;

    [SerializeField] private bool inventoryEnable = false;
    private Vector3 originPos;
    private Vector3 targetPos;

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

        originPos = inventoryPanel.transform.position;
        targetPos = originPos + new Vector3(-798, 0, 0);
    }

    void InventoryEnable()
    {
        inventoryEnable = !inventoryEnable;

        if (inventoryEnable)
        {
            inventoryPanel.transform.DOMove(targetPos, 1.5f).SetEase(Ease.OutQuad);
        }
        else
        {
            inventoryPanel.transform.DOMove(originPos, 1.5f).SetEase(Ease.Flash);
        }

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
