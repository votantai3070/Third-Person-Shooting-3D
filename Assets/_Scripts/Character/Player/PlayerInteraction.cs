// PlayerInteraction.cs
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    private Player player;
    private PlayerControls controls;

    [SerializeField] private List<Interactable> interactList;
    [SerializeField] private Interactable closestInteractable;

    bool hideMissionInfo = true;

    private void Start()
    {
        player = GetComponent<Player>();

        controls = ControlsManager.instance.controls;

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
        player.visuals.SwitchOffWeaponHolder();
        player.anim.SetTrigger("Interact");
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

    public Interactable GetClosestInteractable() => closestInteractable;

    private void AssignInputEvents()
    {
        controls.Player.Interact.performed += ctx =>
        {
            if (player.controller.OnlyTwoWeaponInSlotEquip() || closestInteractable.IsPickupAmmo())
                InteractWithClosest();
        };

        controls.Player.CarEnter.performed += ctx => InteractWithClosest();


        controls.Player.HideMissionInfo.performed += ctx =>
        {
            hideMissionInfo = !hideMissionInfo;
            UI.instance.ingameUI.HandleMissionTooltip(hideMissionInfo);
        };

    }
}
