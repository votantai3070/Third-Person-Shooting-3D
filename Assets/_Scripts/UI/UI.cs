using UnityEngine;

public class UI : MonoBehaviour
{
    public static UI instance;

    public UI_Ingame ingameUI { get; private set; }
    public UI_WeaponSlot weaponSlotUI { get; private set; }
    public UI_WeaponSelection weaponSelectionUI { get; private set; }

    public GameObject[] UIElement;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        ingameUI = GetComponentInChildren<UI_Ingame>(true);
        weaponSlotUI = GetComponentInChildren<UI_WeaponSlot>(true);
        weaponSelectionUI = GetComponentInChildren<UI_WeaponSelection>(true);
    }

    public void ShowUIElement(GameObject go)
    {
        foreach (GameObject element in UIElement)
        {
            element.SetActive(false);
        }

        go.SetActive(true);
    }

    public void StartTheGame()
    {
        ShowUIElement(ingameUI.gameObject);
        GameManager.instance.GameStart();
    }

    public void QuitTheGame() => Application.Quit();
}
