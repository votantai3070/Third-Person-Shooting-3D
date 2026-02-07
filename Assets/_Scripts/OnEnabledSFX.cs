using UnityEngine;

public class OnEnabledSFX : MonoBehaviour
{
    public AudioSource sfx;
    [SerializeField] private float minPitch = .8f;
    [SerializeField] private float maxPitch = 1.1f;
    [SerializeField] private float volume = .05f;

    private void OnEnable()
    {
        PlaySFX();
    }

    private void PlaySFX()
    {
        float pitch = Random.Range(minPitch, maxPitch);

        sfx.volume = volume;
        sfx.pitch = pitch;
        sfx.Play();
    }
}
