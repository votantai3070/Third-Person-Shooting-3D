using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_TransparentOnHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Dictionary<Image, Color> orginalImageColors = new Dictionary<Image, Color>();
    private Dictionary<TextMeshProUGUI, Color> orginalTextColors = new Dictionary<TextMeshProUGUI, Color>();

    private void Start()
    {
        // Cache original colors
        foreach (Image image in GetComponentsInChildren<Image>(true))
        {
            orginalImageColors[image] = image.color;
        }

        // Cache original text colors
        foreach (TextMeshProUGUI text in GetComponentsInChildren<TextMeshProUGUI>(true))
        {
            orginalTextColors[text] = text.color;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("Pointer Entered");

        foreach (Image image in orginalImageColors.Keys)
        {
            var color = image.color;
            color.a = 0.15f;
            image.color = color;
        }

        foreach (TextMeshProUGUI text in orginalTextColors.Keys)
        {
            var color = text.color;
            color.a = 0.15f;
            text.color = color;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("Pointer Exited");

        foreach (var image in orginalImageColors.Keys)
        {
            image.color = orginalImageColors[image];
        }

        foreach (var text in orginalTextColors.Keys)
        {
            text.color = orginalTextColors[text];
        }
    }
}
