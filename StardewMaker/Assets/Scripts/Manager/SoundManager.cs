using UnityEngine;
using UnityEngine.Audio;

public enum Volume
{
    LOW = 1,
    MEDIUM = 10,
    HIGH = 100
}

public class SoundManager : Singleton<SoundManager>
{

    [SerializeField] private AudioSource bgmAudioSource;
    [SerializeField] private AudioSource sfxAudioSource;
    [SerializeField] private AudioClip BGM1;
    [SerializeField] private AudioClip sfxDialog;
    [SerializeField] private AudioMixer audioMixer;

    private string bgm = "BGM";
    private string sfx = "SFX";

    protected override void Awake()
    {
        base.Awake();
    }

    public void PlayBGM1(Volume volume) // 1 ~ 100
    {
        float normalizedVolume = Mathf.Clamp((int)volume, 1f, 100f) * 0.1f; // 0.1 ~ 10
        audioMixer.SetFloat(bgm, Mathf.Log10(normalizedVolume) * 20); // -20dB ~ 20dB
        bgmAudioSource.clip = BGM1;
        bgmAudioSource.Play();
    }

    public void PlaySfxDialog(Volume volume)
    {
        float normalizedVolume = Mathf.Clamp((int)volume, 1f, 100f) * 0.1f; // 0.1 ~ 10
        audioMixer.SetFloat(sfx, Mathf.Log10(normalizedVolume) * 20); // -20dB ~ 20dB
        sfxAudioSource.clip = sfxDialog;
        sfxAudioSource.Play();
    }

    public void StopSfx()
    {
        sfxAudioSource.Stop();
    }

}
