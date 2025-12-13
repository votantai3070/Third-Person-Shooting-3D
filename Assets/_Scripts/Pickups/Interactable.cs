using UnityEngine;

public class Interactable : MonoBehaviour
{
    public void Interact()
    {
        Debug.Log("nhặt súng");
    }

    private void OnTriggerEnter(Collider other)
    {
        PlayerInteraction interaction = other.GetComponent<PlayerInteraction>();

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
