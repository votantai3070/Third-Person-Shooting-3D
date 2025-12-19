using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerControls controls;
    public PlayerMovement movement { get; private set; }
    public PlayerWeaponVisuals visuals { get; private set; }
    public PlayerWeaponControllers controller { get; private set; }
    public PlayerAim aim { get; private set; }
    public Animator anim { get; private set; }


    private void Awake()
    {
        controls = new PlayerControls();

        movement = GetComponent<PlayerMovement>();
        visuals = GetComponent<PlayerWeaponVisuals>();
        controller = GetComponent<PlayerWeaponControllers>();
        aim = GetComponent<PlayerAim>();
        anim = GetComponentInChildren<Animator>();
    }

    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }
}
