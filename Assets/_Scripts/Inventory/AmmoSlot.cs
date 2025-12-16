using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AmmoSlot : MonoBehaviour
{
    [SerializeField] Sprite[] sprites;
    [SerializeField] GameObject ammoSlotPrefab;

    private void Start()
    {
        for (int i = 0; i < sprites.Length; i++)
        {
            Instantiate(ammoSlotPrefab, transform);
        }

        GenerateAmmoSlot();
    }

    private void GenerateAmmoSlot()
    {
        for (int i = 0; i < sprites.Length; i++)
        {
            Image ammoImage = transform.GetChild(i).GetComponent<Image>();
            TextMeshProUGUI ammoAmount = ammoImage.transform.GetChild(0).GetComponent<TextMeshProUGUI>();

            ammoImage.sprite = sprites[i];
            ammoAmount.text = i.ToString();
        }
    }
}
