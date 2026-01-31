using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_WeaponSelection : MonoBehaviour
{
    [SerializeField] private GameObject nextUIToSwitchOn;
    public UI_SelectedWeaponWindow[] seletedWeapon;

    [Header("Warning info")]
    [SerializeField] private TextMeshProUGUI warningText;
    [SerializeField] private float displaySpeed = .25f;
    private float currentWarningAlpha;
    private float targetWarningAlpha;

    private void Start()
    {
        seletedWeapon = GetComponentsInChildren<UI_SelectedWeaponWindow>();
    }

    private void Update()
    {
        if (currentWarningAlpha > targetWarningAlpha)
        {
            currentWarningAlpha -= Time.deltaTime * displaySpeed;
            warningText.color = new Color(1, 1, 1, currentWarningAlpha);
        }
    }

    public void ConfirmWeaponSelection()
    {
        if (AtLeastOneWeaponSeleted())
        {
            UI.instance.ShowUIElement(nextUIToSwitchOn);
            UI.instance.StartLevelGeneration();
        }
        else
            ShowWarningMess("Hãy chọn ít nhất 1 vũ khí");
    }

    private bool AtLeastOneWeaponSeleted() => SelectWeaponData().Count > 0;

    public List<Weapon_Data> SelectWeaponData()
    {
        List<Weapon_Data> selectData = new List<Weapon_Data>();

        foreach (var slot in seletedWeapon)
        {
            if (slot.weaponData != null)
                selectData.Add(slot.weaponData);
        }

        return selectData;
    }

    public UI_SelectedWeaponWindow FindEmptySlot()
    {
        foreach (var slot in seletedWeapon)
        {
            if (slot.IsEmpty())
                return slot;
        }
        return null;
    }

    public UI_SelectedWeaponWindow FindSlotWithWeaponOfType(Weapon_Data weaponData)
    {
        for (int i = 0; i < seletedWeapon.Length; i++)
        {
            if (seletedWeapon[i].weaponData == weaponData)
            {
                return seletedWeapon[i];
            }
        }

        return null;
    }

    public void ShowWarningMess(string message)
    {
        warningText.color = Color.white;
        warningText.text = message;

        currentWarningAlpha = warningText.color.a;
        targetWarningAlpha = 0;
    }
}
