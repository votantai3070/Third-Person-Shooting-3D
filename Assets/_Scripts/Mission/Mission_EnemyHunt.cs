using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "New Mission Enemy Hunt", menuName = "Missions/Mission - Enemy Hunt")]
public class Mission_EnemyHunt : Mission
{
    [SerializeField] private int enemiesToHunt = 10;
    [SerializeField] private EnemyType enemyType;

    int killsTogo;

    public override void StartMission()
    {
        killsTogo = enemiesToHunt;

        MissionObject_Hunt.OnTargetKilled += EliminateKill;

        List<Enemy> validEnemies = new();
        // Gather all enemies of the specified type
        foreach (Enemy enemy in LevelGenerator.instance.GetEnemyList())
        {
            if (enemy.visuals.enemyType == enemyType)
            {
                validEnemies.Add(enemy);
            }
        }

        Debug.Log("Enemy Hunt Mission: Found " + validEnemies.Count + " valid enemies of type " + enemyType + ".");

        for (int i = 0; i < enemiesToHunt; i++)
        {
            if (validEnemies.Count <= 0)
                return;

            int randomIndex = Random.Range(0, validEnemies.Count);
            validEnemies[randomIndex].AddComponent<MissionObject_Hunt>();
            validEnemies.RemoveAt(randomIndex);
        }
    }

    public override bool MissionCompleted()
    {
        return killsTogo <= 0;
    }

    private void EliminateKill()
    {
        killsTogo--;

        Debug.Log("Enemy Hunt Mission: Enemy killed. " + killsTogo + " kills remaining.");

        if (killsTogo <= 0)
        {
            MissionObject_Hunt.OnTargetKilled -= EliminateKill;
        }
    }
}
