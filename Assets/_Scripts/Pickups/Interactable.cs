using UnityEngine;

public class Interactable : MonoBehaviour
{
    protected Player player;
    PlayerInteraction interaction;

    protected bool isAmmoPickup;

    public virtual void Interact()
    {
        Debug.Log(gameObject.name);
    }

    public bool IsPickupAmmo() => isAmmoPickup;

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponentInParent<Player>())
        {
            player = other.GetComponentInParent<Player>();
            interaction = other.GetComponentInParent<PlayerInteraction>();
        }


        interaction.GetInteractables().Add(this);
        interaction.FindClosestInteractable();
    }
    private void OnTriggerExit(Collider other)
    {
        PlayerInteraction interaction = other.GetComponentInParent<PlayerInteraction>();

        if (interaction != null)
        {
            interaction.GetInteractables().Remove(this);
            interaction.FindClosestInteractable();
        }
    }
}
