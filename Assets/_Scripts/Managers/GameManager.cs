using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance { get; private set; }

    public Player player { get; private set; }

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

    public void GameStart()
    {
        SetDefaultWeaponForPlayer();
        LevelGenerator.instance.InitializeGeneration();

        // Start selected mission in a LevelGenerator script, after we done with level generation
    }

    public void SetDefaultWeaponForPlayer()
    {
        List<Weapon_Data> newList = UI.instance.weaponSelectionUI.SelectWeaponData();

        player.controller.SetDefaultWeapon(newList);
    }
}
