using System;
using UnityEngine;

public class MissionObject_Key : MonoBehaviour
{
    public static event Action OnKeyPickedUp;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            OnKeyPickedUp?.Invoke();
            Destroy(gameObject);
        }
    }
}
