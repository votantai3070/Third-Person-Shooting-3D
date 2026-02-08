using System.Collections;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [SerializeField] private AudioSource[] bgm;

    [SerializeField] private bool playBgm;

    private int bgmIndex;

    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        if (!playBgm && BgmIsPlaying())
            StopAllBMG();
        else if (!bgm[bgmIndex].isPlaying)
            PlayRandomMusic();
    }

    public void PlaySFX(AudioSource sfx, bool randomPitch = false, float minPitch = .85f, float maxPitch = 1.1f)
    {
        if (sfx == null)
            return;

        float randomPitchValue = Random.Range(minPitch, maxPitch);

        if (randomPitch)
            sfx.pitch = randomPitchValue;
        sfx.Play();
    }

    public void PlayBMG(int index)
    {
        StopAllBMG();

        bgmIndex = index;
        bgm[index].Play();
    }

    public void SFXDelayAndFade(AudioSource sound, bool play, float targetVolume, float delay, float duration = 1)
        => StartCoroutine(SFXDelayAndFadeCo(sound, play, targetVolume, delay, duration));

    public void StopAllBMG()
    {
        foreach (var bmg in bgm)
        {
            bmg.Stop();
        }
    }

    [ContextMenu("Play Random BMG Music")]
    public void PlayRandomMusic()
    {
        bgmIndex = Random.Range(0, bgm.Length);

        PlayBMG(bgmIndex);
    }

    private bool BgmIsPlaying()
    {
        for (int i = 0; i < bgm.Length; i++)
        {
            if (bgm[i].isPlaying)
                return true;
        }

        return false;
    }

    private IEnumerator SFXDelayAndFadeCo(AudioSource sound, bool play, float targetVolume, float delay = 1, float duration = 1)
    {
        yield return new WaitForSeconds(delay);

        float startVolume = play ? 0 : sound.volume;
        float endVolume = play ? targetVolume : 0;
        float elapsed = 0;

        if (play)
        {
            sound.volume = 0;
            sound.Play();
        }

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            sound.volume = Mathf.Lerp(startVolume, endVolume, elapsed / duration);
            yield return null;
        }

        sound.volume = endVolume;

        if (!play)
            sound.Stop();
    }
}
