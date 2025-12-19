// PlayerInteraction.cs
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    private Player player;

    [SerializeField] private List<Interactable> interactList;
    [SerializeField] private Interactable closestInteractable;

    private void Start()
    {
        player = GetComponent<Player>();
        AssignInputEvents();

        InventoryManager.instance.SetMainWeaponList(player.controller.GetListWeapon());
    }

    void Update()
    {
        UpdateMainWeaponInInventory();
    }

    private void UpdateMainWeaponInInventory()
    {
        List<Weapon> weaponList = player.controller.GetListWeapon();

        if (weaponList != InventoryManager.instance.GetPreviousSlotWeaponList())
        {
            InventoryManager.instance.SetMainWeaponList(weaponList);
            InventoryManager.instance.SetPreviousWeaponList(weaponList);
        }
    }

    private void InteractWithClosest()
    {
        //if (InventoryManager.instance != null)
        //{
        //    InventoryManager.instance.SetMainWeaponList(player.controller.GetListWeapon());
        //}
        closestInteractable?.Interact();
    }

    public void FindClosestInteractable()
    {
        float minDistance = float.MaxValue;

        foreach (var item in interactList)
        {
            float distance = Vector3.Distance(transform.position, item.transform.position);

            if (distance < minDistance)
            {
                minDistance = distance;
                closestInteractable = item;
            }
        }
    }

    public List<Interactable> GetInteractables() => interactList;

    private void AssignInputEvents()
    {
        player.controls.Player.Interact.performed += ctx =>
        {
            if (player.controller.OnlyTwoWeaponInSlotEquip() || closestInteractable.IsPickupAmmo())
                InteractWithClosest();
        };
    }
}
