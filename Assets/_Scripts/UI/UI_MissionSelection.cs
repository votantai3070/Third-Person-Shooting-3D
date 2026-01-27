using TMPro;
using UnityEngine;

public class UI_MissionSelection : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI missionDescription;

    public void UpdateMissionDescription(string mission)
    {
        missionDescription.text = mission;
    }
}
