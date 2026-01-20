using System;
using UnityEngine;

public class MissionObject_CarToDelivery : MonoBehaviour
{
    public static event Action OnCarDelivery;

    public void InvokeOnCarDelivery()
    {
        OnCarDelivery?.Invoke();
    }
}
