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

        UpdateMissionUI();

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

        UI.instance.ingameUI.UpdateMissionUI("Car Delivery - Mission Complete!", "The car has been successfully delivered.");

        MissionObject_CarToDelivery.OnCarDelivery -= HandleCarDelivery;
    }

    private void UpdateMissionUI()
    {
        string missionTitle = "Car Delivery";
        string missionDetails = "Deliver the car to the designated delivery zone.";
        UI.instance.ingameUI.UpdateMissionUI(missionTitle, missionDetails);
    }
}
