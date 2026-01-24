using UnityEngine;

[CreateAssetMenu(fileName = "New Mission Key Find", menuName = "Missions/Mission - Find Key")]
public class Mission_KeyFind : Mission
{
    [SerializeField] private GameObject key;
    private bool keyFound;

    public override void StartMission()
    {
        MissionObject_Key.OnKeyPickedUp += KeyFound;

        UpdateMissionUI();

        Enemy enemy = LevelGenerator.instance.ChooseRandomEnemy();
        enemy.dropController.GiveKey(key);
        enemy.EnemyVIP();
    }

    public override bool MissionCompleted()
    {
        return keyFound;
    }

    public void KeyFound()
    {
        keyFound = true;

        UI.instance.ingameUI.UpdateMissionUI("Mission Complete", "You have found the key!");

        MissionObject_Key.OnKeyPickedUp -= KeyFound;
        Debug.Log("Mission_KeyFind: Key found, mission completed.");
    }
    public void UpdateMissionUI()
    {
        string missionTitle = "Find the Key";
        string missionDetails = "Locate and pick up the key dropped by the VIP enemy.";
        UI.instance.ingameUI.UpdateMissionUI(missionTitle, missionDetails);
    }
}
