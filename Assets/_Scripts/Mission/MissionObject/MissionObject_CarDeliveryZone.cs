using UnityEngine;

public class MissionObject_CarDeliveryZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Car"))
            return;

        Car car = other.GetComponent<Car>();

        car.GetComponent<MissionObject_CarToDelivery>().InvokeOnCarDelivery();
    }
}
