using UnityEngine;

public class Interactable : MonoBehaviour
{
    protected Player player;

    public virtual void Interact()
    {
        Debug.Log(gameObject.name);
    }

    protected void AddAmmo(WeaponType weaponType, int amount)
    {
        AmmoSlotUI[] ammoSlots = AmmoInventoryUI.instance.GetComponentsInChildren<AmmoSlotUI>(true);

        foreach (var ammoSlot in ammoSlots)
        {
            if (weaponType == ammoSlot.GetAmmo().weaponType)
            {
                ammoSlot.AddAmmoAmount(amount);
            }
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        PlayerInteraction interaction = other.GetComponent<PlayerInteraction>();
        player = other.GetComponent<Player>();

        if (interaction != null)
        {
            interaction.GetInteractables().Add(this);
            interaction.FindClosestInteractable();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        PlayerInteraction interaction = other.GetComponent<PlayerInteraction>();

        if (interaction != null)
        {
            interaction.GetInteractables().Remove(this);
            interaction.FindClosestInteractable();
        }
    }
}
