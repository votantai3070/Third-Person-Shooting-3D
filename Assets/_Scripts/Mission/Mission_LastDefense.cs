using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Mission Last Defense", menuName = "Missions/Defense - Mission")]
public class Mission_LastDefense : Mission
{
    public bool defenseActive = false;

    [Header("Cooldown Settings")]
    public float defenseDuration = 120f;
    private float defenseTimer;
    public float waveCooldown = 15f;
    private float waveCooldownTimer;

    [Header("Respawn Mission Details")]
    public int amountOfRespawnPoints = 2;
    public List<Transform> respawnPoints;
    private Vector3 defensePoint;

    [Space]

    public int enemiesPerWave = 5;
    public GameObject[] possibleEnemies;

    private string defenseTimerText;


    public override void StartMission()
    {
        defensePoint = FindAnyObjectByType<MissionEnd_Trigger>().transform.position;
        respawnPoints = new List<Transform>(ClosestPoints(amountOfRespawnPoints));

        defenseActive = false;

        UpdateMissionUI();
    }

    public override void UpdateMission()
    {
        if (!defenseActive)
            return;

        defenseTimer -= Time.deltaTime;
        waveCooldownTimer -= Time.deltaTime;

        if (waveCooldownTimer <= 0f)
        {
            CreateNewEnemies(enemiesPerWave);
            waveCooldownTimer = waveCooldown;
        }

        UpdateMissionUI();

        DefenseSuccess();
    }

    private void DefenseSuccess()
    {
        if (defenseTimer > 0)
            return;

        defenseActive = false;
        UI.instance.ingameUI.UpdateMissionUI("Last Defense - Mission Complete!", "You have successfully defended against the enemy waves.");
    }

    public override bool MissionCompleted()
    {
        if (!defenseActive)
        {
            StartDefenseEvent();
            return false;
        }

        return defenseTimer < 0;
    }

    private void StartDefenseEvent()
    {
        waveCooldownTimer = .5f;
        defenseTimer = defenseDuration;
        defenseActive = true;
    }

    // Spawns a new wave of enemies at random respawn points
    private void CreateNewEnemies(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            int randomEnemyIndex = Random.Range(0, possibleEnemies.Length);
            int randomRespawnIndex = Random.Range(0, respawnPoints.Count);

            Transform randomRespawnPoint = respawnPoints[randomRespawnIndex];
            GameObject randomEnemyPrefab = possibleEnemies[randomEnemyIndex];

            randomEnemyPrefab.GetComponent<Enemy>().chaseRange = 100f;

            ObjectPool.instance.GetObject(randomEnemyPrefab, randomRespawnPoint);
        }
    }

    // Finds the closest enemy respawn points to the defense point
    private List<Transform> ClosestPoints(int amount)
    {
        List<Transform> closestPoints = new List<Transform>();
        List<MissionObject_EnemyRespawn> allPoints = new List<MissionObject_EnemyRespawn>(
            FindObjectsByType<MissionObject_EnemyRespawn>(FindObjectsSortMode.None)
        );

        while (closestPoints.Count < amount && allPoints.Count > 0)
        {
            MissionObject_EnemyRespawn closestPoint = null;
            float closestDistance = float.MaxValue;
            foreach (var point in allPoints)
            {
                float distance = Vector3.Distance(defensePoint, point.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestPoint = point;
                }
            }

            if (closestPoint != null)
            {
                closestPoints.Add(closestPoint.transform);
                allPoints.Remove(closestPoint);
            }
            else
            {
                break; // No more points available
            }
        }

        return closestPoints;
    }

    private void UpdateMissionUI()
    {
        string missionTitle = "Last Defense";
        string missionDetails = "Survive the enemy waves for " + System.TimeSpan.FromSeconds(defenseDuration).ToString("mm':'ss") + ".";
        UI.instance.ingameUI.UpdateMissionUI(missionTitle, missionDetails);
    }

}
