using UnityEngine;
using UnityEngine.EventSystems;

public class UI_HandleButton : MonoBehaviour, IPointerEnterHandler, IPointerDownHandler
{
    [Header("Ui Audio")]
    [SerializeField] private AudioSource pointerEnterSFX;
    [SerializeField] private AudioSource pointerDownSFX;


    public void OnPointerEnter(PointerEventData eventData)
    {
        if (pointerEnterSFX != null)
            pointerEnterSFX.Play();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (pointerDownSFX != null)
            pointerDownSFX.Play();
    }
}
