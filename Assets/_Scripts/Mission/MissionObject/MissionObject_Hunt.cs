using System;
using UnityEngine;

public class MissionObject_Hunt : MonoBehaviour
{
    public static event Action OnTargetKilled;

    public void InvokeTargetKilled()
    {
        OnTargetKilled?.Invoke();
    }
}
