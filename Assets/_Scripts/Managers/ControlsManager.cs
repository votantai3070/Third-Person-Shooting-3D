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
        controls = GameManager.instance.player.controls;
        player = GameManager.instance.player;

        SwitchToCharacterControls();
    }

    public void SwitchToCharacterControls()
    {
        controls.Player.Enable();
        controls.UI.Disable();
        player.SetControlsEnabledTo(true);
    }

    public void SwitchToUIControls()
    {
        controls.Player.Disable();
        controls.UI.Enable();
        player.SetControlsEnabledTo(false);
    }
}
