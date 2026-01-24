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
}
