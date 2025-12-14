using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] private List<Interactable> interactList;
    [SerializeField] private Interactable closestInteractable;

    private void Start()
    {
        Player player = GetComponent<Player>();
        player.controls.Player.Interact.performed += ctx => InteractWithClosest();
    }

    private void InteractWithClosest()
    {
        closestInteractable?.Interact();
    }

    public void FindClosestInteractable()
    {
        float maxDistance = float.MaxValue;

        foreach (var item in interactList)
        {
            float distance = Vector3.Distance(transform.position, item.transform.position);

            maxDistance = distance;
            closestInteractable = item;
        }
    }

    public List<Interactable> GetInteractables() => interactList;
}
