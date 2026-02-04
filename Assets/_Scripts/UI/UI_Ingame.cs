using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Ingame : MonoBehaviour
{
    [SerializeField] GameObject characterUI;
    [SerializeField] GameObject carUI;

    [Header("Health Info")]
    [SerializeField] private Image healthBar;
    [SerializeField] private TextMeshProUGUI healthText;

    [Header("Mission Info")]
    [SerializeField] private TextMeshProUGUI missionText;
    [SerializeField] private TextMeshProUGUI missionDetail;

    [Header("Tooltip Mission Info")]
    [SerializeField] private GameObject missionTooltip;
    [SerializeField] private GameObject missionInfo;

    [Header("Car Info")]
    [SerializeField] private TextMeshProUGUI carSpeedText;
    [SerializeField] private Image carHealthBar;
    [SerializeField] private TextMeshProUGUI carHealthText;

    private void Start()
    {
        HandleMissionTooltip(true);
    }

    public void SwitchToCharacterUI()
    {
        characterUI.SetActive(true);
        carUI.SetActive(false);
    }

    public void SwitchToCarUI()
    {
        carUI.SetActive(true);
        characterUI.SetActive(false);
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

    public void UpdateCarHealthUI(float currentHealth, float maxHealth)
    {
        carHealthBar.fillAmount = currentHealth / maxHealth;
        carHealthText.text = $"{currentHealth} / {maxHealth}";
    }

    public void UpdateSpeedText(float currentSpeed)
    {
        carSpeedText.text = currentSpeed.ToString() + " km/h";
    }

}
