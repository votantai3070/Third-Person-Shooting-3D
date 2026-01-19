using UnityEngine;

[CreateAssetMenu(fileName = "New Timer Mission", menuName = "Missions/Timer mission")]
public class MissionTimer : Mission
{
    public float time;

    private float currentTime;

    public override void StartMission()
    {
        currentTime = time;
    }

    public override void UpdateMission()
    {
        base.UpdateMission();

        currentTime -= Time.deltaTime;

        if (currentTime < 0)
            Debug.Log("Over Time");

        string timeStr = System.TimeSpan.FromSeconds(currentTime).ToString("mm':'ss");

        Debug.Log(timeStr);
    }

    public override bool MissionCompleted()
    {
        return currentTime > 0;
    }
}
