using UnityEngine;

public class Interactable : MonoBehaviour
{
    Player player;

    public void Interact()
    {
        Debug.Log(gameObject.name);

        Weapon weapon = GetComponent<PickupWeapon>().weapon;

        player.controller.GetListWeapon().Add(weapon);
        ObjectPool.instance.DelayReturnToPool(gameObject);
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
