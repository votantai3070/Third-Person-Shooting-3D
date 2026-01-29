using System.Collections;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public static TimeManager instance { get; private set; }

    [SerializeField] private float resumeRate = 3;
    [SerializeField] private float pauseRate = 7;

    private float timeAdjustRate;
    private float targetTimeScale = 1f;

    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        if (Mathf.Abs(Time.timeScale - targetTimeScale) > .05f)
        {
            float adjustRate = timeAdjustRate * Time.unscaledDeltaTime;
            Time.timeScale = Mathf.Lerp(Time.timeScale, targetTimeScale, adjustRate);
        }
        else
        {
            Time.timeScale = targetTimeScale;
        }
    }

    public void PauseTime()
    {
        timeAdjustRate = pauseRate;
        targetTimeScale = 0f;
    }

    public void ResumeTime()
    {
        timeAdjustRate = resumeRate;
        targetTimeScale = 1f;
    }

    public void SlowMotionFor(float second)
    {
        StartCoroutine(SlowTimeCo(second));
    }

    private IEnumerator SlowTimeCo(float second)
    {
        targetTimeScale = .5f;
        Time.timeScale = targetTimeScale;
        yield return new WaitForSecondsRealtime(second);
        ResumeTime();
    }
}
