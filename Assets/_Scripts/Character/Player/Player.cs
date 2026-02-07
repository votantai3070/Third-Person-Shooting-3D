using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerControls controls;
    public PlayerMovement movement { get; private set; }
    public PlayerWeaponVisuals visuals { get; private set; }
    public PlayerWeaponControllers controller { get; private set; }
    public PlayerAim aim { get; private set; }
    public PlayerInteraction interaction { get; private set; }
    public Animator anim { get; private set; }
    public PlayerDead dead { get; private set; }
    public Ragdoll ragdoll { get; private set; }
    public bool controlsEnabled { get; private set; }
    public Player_SoundFX sound { get; private set; }
    public Player_HealthController healthController { get; private set; }

    private void Awake()
    {
        movement = GetComponent<PlayerMovement>();
        visuals = GetComponent<PlayerWeaponVisuals>();
        controller = GetComponent<PlayerWeaponControllers>();
        aim = GetComponent<PlayerAim>();
        interaction = GetComponent<PlayerInteraction>();
        anim = GetComponentInChildren<Animator>();
        dead = GetComponent<PlayerDead>();
        ragdoll = GetComponent<Ragdoll>();
        sound = GetComponent<Player_SoundFX>();
        healthController = GetComponent<Player_HealthController>();
    }

    private void Start()
    {
        controls = ControlsManager.instance.controls;
    }

    private void OnEnable()
    {
        anim.enabled = true;

        controls.Player.Pause.performed += ctx => UI.instance.PauseSwitch();
    }

    public void SetControlsEnabledTo(bool enabled)
    {
        controlsEnabled = enabled;
        ragdoll.CollidersActive(enabled); // Important: Disabled collider when into car
    }

    public void Die()
    {
        dead.PlayerAnimationDead();
    }
}
