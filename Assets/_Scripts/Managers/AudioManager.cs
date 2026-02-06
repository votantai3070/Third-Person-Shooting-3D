using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource[] bgm;

    [SerializeField] private bool playBgm;

    private int bgmIndex;

    private void Update()
    {
        if (!playBgm && BgmIsPlaying())
            StopAllBMG();
        else if (!bgm[bgmIndex].isPlaying)
            PlayRandomMusic();
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
