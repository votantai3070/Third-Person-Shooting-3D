using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_ComicPanel : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] private Image[] comicPanel;
    [SerializeField] private int imageIndex;
    [SerializeField] private GameObject playButton;

    private Image myImage;
    [SerializeField] private bool comicShowOver;
    private bool isFadeIn;

    [SerializeField] float fadeDuration = 1.5f;

    private void Start()
    {
        foreach (var panel in comicPanel)
        {
            Color color = panel.color;
            color.a = 0f;
            panel.color = color;
        }

        ShowNextPanel();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (comicShowOver || isFadeIn) return;
        ShowNextPanel();
    }

    private void ShowNextPanel()
    {
        if (imageIndex >= comicPanel.Length)
        {
            comicShowOver = true;
            playButton.SetActive(true);
            return;
        }

        isFadeIn = true;

        comicPanel[imageIndex].DOFade(1f, fadeDuration).SetUpdate(true).OnComplete(() =>
        {
            isFadeIn = false;
            imageIndex++;

            if (imageIndex >= comicPanel.Length)
            {
                comicShowOver = true;
                if (playButton != null)
                    playButton.SetActive(true);
                return;
            }
        });
    }
}
