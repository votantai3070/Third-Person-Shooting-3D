using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    public static UI instance;

    public UI_Ingame ingameUI { get; private set; }
    public UI_WeaponSlot weaponSlotUI { get; private set; }
    public UI_WeaponSelection weaponSelectionUI { get; private set; }
    public UI_GameOver gameOverUI { get; private set; }

    public GameObject pauseUI;
    public GameObject[] UIElement;

    [Header("Fade Settings")]
    [SerializeField] private Image fadeImage;
    [SerializeField] private float fadeInDuration = 1f;
    [SerializeField] private float fadeOutDuration = 1f;

    private Tween currentFadeTween; // Track fade tween để kill nếu cần

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
        gameOverUI = GetComponentInChildren<UI_GameOver>(true);
    }

    private void Start()
    {
        AssignInputsUI();

        FadeIn();
    }

    // Fade In - Hiện game (alpha 1 → 0)
    public void FadeIn(System.Action onComplete = null)
    {
        // Kill fade cũ nếu đang chạy
        currentFadeTween?.Kill();

        currentFadeTween = fadeImage.DOFade(0f, fadeInDuration)
            .SetUpdate(true) // Dùng unscaled time (không bị ảnh hưởng bởi Time.timeScale)
            .OnComplete(() =>
            {
                fadeImage.raycastTarget = false; // Tắt block clicks
                onComplete?.Invoke();
            });
    }

    // Fade Out - Che màn hình (alpha 0 → 1)
    public void FadeOut(System.Action onComplete = null)
    {
        currentFadeTween?.Kill();

        fadeImage.raycastTarget = true; // Bật block clicks

        currentFadeTween = fadeImage.DOFade(1f, fadeOutDuration)
            .SetUpdate(true)
            .OnComplete(() => onComplete?.Invoke());
    }

    // Restart với fade effect
    public void RestartTheGame()
    {
        FadeOut(() => GameManager.instance.RestartScene());
    }

    // Load scene với fade
    public void LoadSceneWithFade(string sceneName)
    {
        FadeOut(() =>
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
        });
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
        StartGameSequence();
    }

    private void StartGameSequence()
    {
        Sequence startSequence = DOTween.Sequence();

        startSequence.Append(fadeImage.DOFade(1f, fadeOutDuration)) // Fade out
            .AppendCallback(() =>
            {
                GameManager.instance.GameStart();
                ShowUIElement(ingameUI.gameObject);
            })
            .Append(fadeImage.DOFade(0f, fadeInDuration)) // Fade in
            .SetUpdate(true);
    }

    public void QuitTheGame() => Application.Quit();

    public void PauseSwitch()
    {
        bool gamePaused = pauseUI.activeSelf;

        if (gamePaused)
        {
            ShowUIElement(ingameUI.gameObject);
            ControlsManager.instance.SwitchToCharacterControls();
            TimeManager.instance.ResumeTime();
        }
        else
        {
            ShowUIElement(pauseUI);
            ControlsManager.instance.SwitchToUIControls();
            TimeManager.instance.PauseTime();
        }
    }

    public void ShowGameOverUI(string mess = "GAME OVER!")
    {
        ShowUIElement(gameOverUI.gameObject);
        gameOverUI.ShowGameOverMess(mess);
    }

    private void AssignInputsUI()
    {
        PlayerControls controls = GameManager.instance.player.controls;
        controls.UI.Pause.performed += ctx => PauseSwitch();
    }

    // Clean up khi destroy
    private void OnDestroy()
    {
        currentFadeTween?.Kill();
    }
}
