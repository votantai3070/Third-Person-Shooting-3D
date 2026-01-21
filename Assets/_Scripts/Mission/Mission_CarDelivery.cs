using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "Mission Car Delivery", menuName = "Missions/Car Delivery - Mission")]
public class Mission_CarDelivery : Mission
{
    private bool isCarDelivered;

    public override void StartMission()
    {
        // Activate all car delivery zones
        var deliveryZones = FindObjectsByType<MissionObject_CarDeliveryZone>(FindObjectsInactive.Include, FindObjectsSortMode.None);

        foreach (var zone in deliveryZones)
        {
            zone.gameObject.SetActive(true);
        }

        isCarDelivered = false;

        MissionObject_CarToDelivery.OnCarDelivery += HandleCarDelivery;

        // Add MissionObject_CarToDelivery component to all cars
        Car[] cars = FindObjectsByType<Car>(FindObjectsSortMode.None);

        foreach (var car in cars)
        {
            car.AddComponent<MissionObject_CarToDelivery>();
        }
    }

    public override bool MissionCompleted()
    {
        return isCarDelivered;
    }

    private void HandleCarDelivery()
    {
        isCarDelivered = true;

        MissionObject_CarToDelivery.OnCarDelivery -= HandleCarDelivery;
    }

}
