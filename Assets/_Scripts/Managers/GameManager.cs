using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance { get; private set; }

    public Player player { get; private set; }

    [Space]
    public bool quickStart;
    public bool isPlayerView;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        player = FindAnyObjectByType<Player>();
    }


    public void GameCompleted()
    {
        UI.instance.LoadGameWinUI();
        ControlsManager.instance.controls.Player.Disable();
    }

    public void GameStart()
    {
        SetDefaultWeaponForPlayer();

        //LevelGenerator.instance.InitializeGeneration();
        // Start selected mission in a LevelGenerator script, after we done with level generation
    }

    public void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void GameOver()
    {
        TimeManager.instance.SlowMotionFor(1.5f);
        UI.instance.ShowGameOverUI();
    }

    public void SetDefaultWeaponForPlayer()
    {
        List<Weapon_Data> newList = UI.instance.weaponSelectionUI.SelectWeaponData();

        player.controller.SetDefaultWeapon(newList);
    }
}
