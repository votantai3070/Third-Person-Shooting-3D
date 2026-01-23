using UnityEngine;

public class Player : Character
{
    public PlayerControls controls;
    public PlayerMovement movement { get; private set; }
    public PlayerWeaponVisuals visuals { get; private set; }
    public PlayerWeaponControllers controller { get; private set; }
    public PlayerAim aim { get; private set; }
    public PlayerInteraction interaction { get; private set; }
    public Animator anim { get; private set; }
    public PlayerDead dead { get; private set; }


    private void Awake()
    {
        controls = new PlayerControls();

        movement = GetComponent<PlayerMovement>();
        visuals = GetComponent<PlayerWeaponVisuals>();
        controller = GetComponent<PlayerWeaponControllers>();
        aim = GetComponent<PlayerAim>();
        interaction = GetComponent<PlayerInteraction>();
        anim = GetComponentInChildren<Animator>();
        dead = GetComponent<PlayerDead>();
    }

    private void OnEnable()
    {
        anim.enabled = true;

        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }

    protected override void Die()
    {
        base.Die();

        dead.PlayerAnimationDead();
    }
}
