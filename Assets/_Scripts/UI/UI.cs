using UnityEngine;

public class UI : MonoBehaviour
{
    public static UI instance;

    public UI_Ingame ingameUI { get; private set; }
    public UI_WeaponSlot weaponSlotUI { get; private set; }
    public UI_WeaponSelection weaponSelectionUI { get; private set; }
    public UI_GameOver gameOverUI { get; private set; }

    public GameObject pauseUI;


    public GameObject[] UIElement;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        ingameUI = GetComponentInChildren<UI_Ingame>(true);
        weaponSlotUI = GetComponentInChildren<UI_WeaponSlot>(true);
        weaponSelectionUI = GetComponentInChildren<UI_WeaponSelection>(true);
        gameOverUI = GetComponentInChildren<UI_GameOver>(true);
    }

    private void Start()
    {
        AssignInputsUI();
    }

    public void ShowUIElement(GameObject go)
    {
        foreach (GameObject element in UIElement)
        {
            element.SetActive(false);
        }

        go.SetActive(true);
    }

    public void StartTheGame()
    {
        ShowUIElement(ingameUI.gameObject);
        GameManager.instance.GameStart();
    }

    public void QuitTheGame() => Application.Quit();


    public void PauseSwitch()
    {
        bool gamePaused = pauseUI.activeSelf;

        if (gamePaused)
        {
            ShowUIElement(ingameUI.gameObject);
            ControlsManager.instance.SwitchToCharacterControls();
            TimeManager.instance.ResumeTime();
        }
        else
        {
            ShowUIElement(pauseUI);
            ControlsManager.instance.SwitchToUIControls();
            TimeManager.instance.PauseTime();
        }
    }

    public void ShowGameOverUI(string mess = "GAME OVER!")
    {
        ShowUIElement(gameOverUI.gameObject);
        gameOverUI.ShowGameOverMess(mess);
    }

    private void AssignInputsUI()
    {
        PlayerControls controls = GameManager.instance.player.controls;

        controls.UI.Pause.performed += ctx => PauseSwitch();
    }
}
