using UnityEngine;

public class Car_Sound : MonoBehaviour
{
    public Car_Controller car { get; private set; }

    [SerializeField] private AudioSource carOffSfx;
    [SerializeField] private AudioSource carOnSfx;
    [SerializeField] private AudioSource carWorkSfx;

    [SerializeField] private float minPitch = .75f;
    [SerializeField] private float maxPitch = 1.5f;
    [SerializeField] private float carWordVolume = .07f;

    private bool allowCarSound;

    private void Start()
    {
        car = GetComponent<Car_Controller>();
        Invoke(nameof(AllowCarSound), 1);
    }

    private void Update()
    {
        UpdateCarSound();
    }

    private void UpdateCarSound()
    {
        float currentSpeed = car.speed;

        float pitch = Mathf.Lerp(minPitch, maxPitch, currentSpeed / car.maxSpeed);
        carWorkSfx.pitch = pitch;
    }

    public void PlayCarSoundSfx(bool active)
    {
        if (!allowCarSound)
            return;

        if (active)
        {
            carOnSfx.Play();
            AudioManager.instance.SFXDelayAndFade(carWorkSfx, true, carWordVolume, 1);
        }
        else
        {
            AudioManager.instance.SFXDelayAndFade(carWorkSfx, false, 0, .25f);
            carOffSfx.Play();
        }
    }

    private bool AllowCarSound() => allowCarSound = true;
}
