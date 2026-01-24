using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Ingame : MonoBehaviour
{
    [Header("Health Info")]
    [SerializeField] private Image healthBar;
    [SerializeField] private TextMeshProUGUI healthText;

    [Header("Mission Info")]
    [SerializeField] private TextMeshProUGUI missionText;
    [SerializeField] private TextMeshProUGUI missionDetail;

    [Header("Tooltip Mission Info")]
    [SerializeField] private GameObject missionTooltip;
    [SerializeField] private GameObject missionInfo;

    private void Start()
    {
        HandleMissionTooltip(true);
    }

    public void UpdateHealthUI(float currentHealth, float maxHealth)
    {
        healthBar.fillAmount = currentHealth / maxHealth;
        healthText.text = $"{currentHealth} / {maxHealth}";
    }

    public void UpdateMissionUI(string missionTitle, string missionDetails)
    {
        missionText.text = missionTitle;
        missionDetail.text = missionDetails;
    }

    public void HandleMissionTooltip(bool show)
    {
        missionTooltip.SetActive(show);
        missionInfo.SetActive(!show);
    }
}
