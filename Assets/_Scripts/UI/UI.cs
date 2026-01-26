using UnityEngine;

public class UI : MonoBehaviour
{
    public static UI instance;

    public UI_Ingame ingameUI { get; private set; }
    public UI_WeaponSlot weaponSlotUI { get; private set; }

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
        weaponSlotUI = GetComponentInChildren<UI_WeaponSlot>();
    }

    public void ShowUIElement(GameObject go)
    {
        foreach (GameObject element in UIElement)
        {
            element.SetActive(true);
        }

        go.SetActive(true);
    }
}
