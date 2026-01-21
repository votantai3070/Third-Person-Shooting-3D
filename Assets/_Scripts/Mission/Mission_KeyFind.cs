using UnityEngine;

[CreateAssetMenu(fileName = "New Mission Key Find", menuName = "Missions/Mission - Find Key")]
public class Mission_KeyFind : Mission
{
    [SerializeField] private GameObject key;
    private bool keyFound;

    public override void StartMission()
    {
        MissionObject_Key.OnKeyPickedUp += KeyFound;

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

        MissionObject_Key.OnKeyPickedUp -= KeyFound;
        Debug.Log("Mission_KeyFind: Key found, mission completed.");
    }
}
