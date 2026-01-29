using TMPro;
using UnityEngine;

public class UI_GameOver : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI gameOverText;

    public void ShowGameOverMess(string reason)
    {
        //gameOverText.text = $"Game Over!\n{reason}";
        gameOverText.text = reason;
        //gameObject.SetActive(true);
    }
}
