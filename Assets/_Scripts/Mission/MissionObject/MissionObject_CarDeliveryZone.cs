using UnityEngine;

public class MissionObject_CarDeliveryZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Car"))
            return;

        Car_Controller car = other.GetComponent<Car_Controller>();

        car.GetComponent<MissionObject_CarToDelivery>().InvokeOnCarDelivery();
    }
}
