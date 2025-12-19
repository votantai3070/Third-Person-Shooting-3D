using UnityEngine;

public class PlayerAim : MonoBehaviour
{
    [Header("Aiming")]
    [SerializeField] private bool isAiming = false;

    public void SetAiming(bool aiming)
    {
        isAiming = aiming;
        Crosshair.instance.SetVisible(aiming);
    }

    public bool IsAiming() => isAiming;
}
