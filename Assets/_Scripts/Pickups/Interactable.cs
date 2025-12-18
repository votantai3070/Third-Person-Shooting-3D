using UnityEngine;

public class Interactable : MonoBehaviour
{
    protected Player player;

    protected bool isAmmoPickup;

    public virtual void Interact()
    {
        Debug.Log(gameObject.name);
    }

    public bool IsPickupAmmo() => isAmmoPickup;

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
