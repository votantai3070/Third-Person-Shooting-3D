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
}
