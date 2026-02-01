using UnityEngine;

public class ControlsManager : MonoBehaviour
{
    public static ControlsManager instance { get; private set; }
    public PlayerControls controls { get; private set; }
    private Player player;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        controls = new PlayerControls();
        player = GameManager.instance.player;

        SwitchToCharacterControls();
    }

    public void SwitchToCharacterControls()
    {
        controls.Player.Enable();

        controls.Car.Disable();
        controls.UI.Disable();

        GameManager.instance.isPlayerView = true;

        player.SetControlsEnabledTo(true);
    }

    public void SwitchToUIControls()
    {
        controls.UI.Enable();

        controls.Player.Disable();
        controls.Car.Enable();

        GameManager.instance.isPlayerView = true;

        player.SetControlsEnabledTo(false);
    }

    public void SwitchToCarControl()
    {
        controls.Car.Enable();

        controls.Player.Disable();
        controls.UI.Disable();

        GameManager.instance.isPlayerView = false;

        player.SetControlsEnabledTo(false);
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
