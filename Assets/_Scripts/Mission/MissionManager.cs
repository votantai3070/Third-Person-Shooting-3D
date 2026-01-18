using UnityEngine;

public class MissionManager : MonoBehaviour
{
    public static MissionManager instance;

    public Mission currentMission;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
    }

    private void Start()
    {
        Invoke(nameof(StartMisstion), 2);
    }

    private void Update()
    {
        currentMission?.UpdateMission();
    }

    private void StartMisstion() => currentMission.StartMission();

    public bool MissionCompleted() => currentMission.MissionCompleted();
}
