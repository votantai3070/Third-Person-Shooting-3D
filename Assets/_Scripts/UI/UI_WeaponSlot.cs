using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_WeaponSlot : MonoBehaviour
{
    [SerializeField] private Image weaponImage;
    [SerializeField] private TextMeshProUGUI ammoText;

    public void UpdateWeaponUI(Sprite weaponSprite, int currentAmmo, int maxAmmo)
    {
        if (weaponImage == null || ammoText == null)
            return;

        weaponImage.sprite = weaponSprite;
        ammoText.text = $"{currentAmmo} / {maxAmmo}";
    }
}
