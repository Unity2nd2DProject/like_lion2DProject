using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;  // SceneManager 사용을 위해 추가
using System.Collections.Generic;

public enum Volume
{
    LOW = 1,
    MEDIUM = 5,
    HIGH = 10

}

[System.Serializable]
public class SceneBGMData
{
    public string sceneName;
    public AudioClip bgmClip;
}

public class SoundManager : Singleton<SoundManager>
{

    [SerializeField] private AudioSource bgmAudioSource;
    [SerializeField] private AudioSource sfxAudioSource;
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private AudioClip sfxDialog;

    [Header("씬별 BGM 설정")]
    [SerializeField] private List<SceneBGMData> sceneBGMs;

    private string bgm = "BGM";
    private string sfx = "SFX";

    private bool isBGMMuted = false;
    private bool isSFXMuted = false;
    private float lastBGMVolume = 1f;
    private float lastSFXVolume = 1f;

    protected override void Awake()
    {
        base.Awake();
        SceneManager.sceneLoaded += OnSceneLoaded;  // 이벤트 구독
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;  // 이벤트 구독 해제
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        PlaySceneBGM(scene.name, Volume.MEDIUM); // 기본 볼륨으로 재생
    }

    // 씬별 BGM 재생 (볼륨 조절 가능)
    private void PlaySceneBGM(string sceneName, Volume volume = Volume.MEDIUM)
    {
        var bgmData = sceneBGMs.Find(x => x.sceneName == sceneName);
        if (bgmData != null)
        {
            bgmAudioSource.Stop();
            bgmAudioSource.clip = bgmData.bgmClip;
            SetBGMVolume(volume);
            bgmAudioSource.Play();
        }
    }

    // BGM 볼륨 조절
    public void SetBGMVolume(Volume volume)
    {
        float normalizedVolume = Mathf.Clamp((int)volume, 1f, 100f) * 0.1f;
        audioMixer.SetFloat(bgm, Mathf.Log10(normalizedVolume) * 20);
    }

    // 슬라이더용 BGM 볼륨 조절 (0~1 값)
    public void SetBGMVolumeBySlider(float sliderValue)
    {
        float volume = Mathf.Lerp(1f, 100f, sliderValue);
        SetBGMVolume((Volume)volume);
    }

    // 효과음 재생 (볼륨 조절 가능)
    public void PlaySfxDialog(Volume volume = Volume.MEDIUM)
    {
        float normalizedVolume = Mathf.Clamp((int)volume, 1f, 100f) * 0.1f;
        audioMixer.SetFloat(sfx, Mathf.Log10(normalizedVolume) * 20);
        sfxAudioSource.clip = sfxDialog;
        sfxAudioSource.Play();
    }

    // 슬라이더용 효과음 볼륨 조절 (0~1 값)
    public void SetSFXVolumeBySlider(float sliderValue)
    {
        float volume = Mathf.Lerp(1f, 100f, sliderValue);
        float normalizedVolume = Mathf.Clamp(volume, 1f, 100f) * 0.1f;
        audioMixer.SetFloat(sfx, Mathf.Log10(normalizedVolume) * 20);
    }

    public void StopSfx()
    {
        sfxAudioSource.Stop();
    }

    // BGM 음소거 토글
    public void ToggleBGMMute()
    {
        isBGMMuted = !isBGMMuted;
        if (isBGMMuted)
        {
            // 현재 볼륨 저장 후 음소거
            audioMixer.GetFloat(bgm, out float currentVolume);
            lastBGMVolume = currentVolume;
            audioMixer.SetFloat(bgm, -80f); // -80dB는 실질적인 음소거
        }
        else
        {
            // 저장된 볼륨으로 복구
            audioMixer.SetFloat(bgm, lastBGMVolume);
        }
    }

    // 효과음 음소거 토글
    public void ToggleSFXMute()
    {
        isSFXMuted = !isSFXMuted;
        if (isSFXMuted)
        {
            audioMixer.GetFloat(sfx, out float currentVolume);
            lastSFXVolume = currentVolume;
            audioMixer.SetFloat(sfx, -80f);
        }
        else
        {
            audioMixer.SetFloat(sfx, lastSFXVolume);
        }
    }

    // 음소거 상태 확인
    public bool IsBGMMuted() => isBGMMuted;
    public bool IsSFXMuted() => isSFXMuted;

}
