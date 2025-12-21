using UnityEngine;

public class PlayerEvents : MonoBehaviour
{
    public Player player { get; private set; }

    private void Awake()
    {
        player = GetComponentInParent<Player>();
    }

    public void ShootingEnd()
    {
        player.anim.SetBool("Shooting", false);
    }

    public void ReloadIsOver()
    {
        ReturnRig();
        //Refill bullets
        player.controller.CurrentWeapon().RefillBullets();

        player.controller.SetWeaponReady(true);
    }

    public void ReturnRig()
    {
        player.visuals.MaximizeRigWeight();
        player.visuals.MaximizeLeftHandWeight();
    }

    public void ReduceRig()
    {
        player.visuals.ReduceRigWeight();
        player.visuals.ReduceLeftHandIKWeight();
    }

    public void Interaction()
    {
        player.interaction.GetClosestInteractable()?.Interact();
        player.visuals.SwitchOnWeaponHolder();
    }
}
