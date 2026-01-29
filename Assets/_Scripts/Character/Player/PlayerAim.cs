using UnityEngine;

public class PlayerAim : MonoBehaviour
{
    [Header("Aiming")]
    [SerializeField] private bool isAiming = false;
    private Player player;

    private void Start()
    {
        player = GetComponent<Player>();
    }

    public void SetAiming(bool aiming)
    {
        isAiming = aiming;
        Crosshair.instance.SetVisible(aiming);
    }

    public bool IsAiming() => isAiming;
}
