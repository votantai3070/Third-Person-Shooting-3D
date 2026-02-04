using UnityEngine;

public class Interactable : MonoBehaviour
{
    protected Player player;

    protected bool isAmmoPickup;

    private void Start()
    {
        player = FindAnyObjectByType<Player>().GetComponent<Player>();
    }

    public virtual void Interact()
    {
        Debug.Log(gameObject.name);
    }

    public bool IsPickupAmmo() => isAmmoPickup;

    private void OnTriggerEnter(Collider other)
    {
        PlayerInteraction interaction = other.GetComponentInParent<PlayerInteraction>();

        Debug.Log("Player entered interaction zone of " + gameObject.name);

        if (interaction != null)
        {
            interaction.GetInteractables().Add(this);
            interaction.FindClosestInteractable();
        }
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
