using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;  // SceneManager 사용을 위해 추가
using System.Collections.Generic;
using System;
using UnityEngine.Rendering;



// 날씨별 사운드 데이터를 저장하는 클래스 추가
[System.Serializable]
public class WeatherSoundData
{
    public WeatherType weatherType;     // 날씨 유형
    public AudioClip weatherBGM;        // 해당 날씨의 BGM
    public AudioClip weatherAmbientSFX; // 해당 날씨의 환경음(비 소리 등)
    [Range(0f, 1f)]
    public float ambientVolume = 0.5f;  // 환경음 볼륨
}

[System.Serializable]
public class SceneBGMData
{
    public string sceneName;
    public AudioClip bgmClip;
}

public class SoundManager : Singleton<SoundManager>
{
    [SerializeField] private SFXLibrary sfxLibrary; // ScriptableObject 참조
    [SerializeField] private AudioSource bgmAudioSource;
    [SerializeField] private AudioSource sfxDialogAudioSource;
    [SerializeField] private AudioSource sfxAudioSource;
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private AudioClip sfxDialog;

    [Header("씬별 BGM 설정")]
    [SerializeField] private List<SceneBGMData> sceneBGMs;

    [Header("날씨별 사운드 설정")]
    [SerializeField] private List<WeatherSoundData> weatherSounds;
    [SerializeField] private AudioSource weatherAmbientSource; // 비 소리, 바람 소리 등 환경음 재생용


    private string bgm = "BGM";
    private string sfx = "SFX";

    private bool isBGMMuted = false;
    private bool isSFXMuted = false;

    private string currentSceneName;
    private WeatherType currentWeather = WeatherType.Sunny;

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
        currentSceneName = scene.name;


        SetVolume();
        PlaySceneBGM(scene.name);

        if (WeatherManager.Instance != null)
        {
            UpdateWeatherSound(WeatherManager.Instance.GetCurrentWeather());
        }
        else
        {

            ResetWeatherSound();

        }


    }

    private void SetVolume()
    {
        SetBGMVolumeBySlider(PlayerPrefs.GetFloat("BGMVolume")); // 저장된 BGM 볼륨으로 복원
        SetSFXVolumeBySlider(PlayerPrefs.GetFloat("SFXVolume")); // 저장된 SFX 볼륨으로 복원

        if (isBGMMuted)
        {
            audioMixer.SetFloat(bgm, -80f); // 음소거
        }
        if (isSFXMuted)
        {
            audioMixer.SetFloat(sfx, -80f); // 음소거
        }
    }

    // 날씨에 따른 사운드 설정
    public void UpdateWeatherSound(WeatherType weatherType)
    {
        currentWeather = weatherType;

        // GameManager가 HOME 모드인 경우 날씨 사운드 적용 안함
        if (GameManager.Instance != null && GameManager.Instance.currentMode == GameMode.HOME)
        {
            // HOME 모드에서는 날씨 사운드 리셋 (기본 BGM 사용)
            ResetWeatherSound();
            return;
        }

        // TOWN 모드이거나 GameManager가 없는 경우 날씨 사운드 적용
        // 날씨 타입에 맞는 사운드 데이터 찾기
        WeatherSoundData soundData = weatherSounds.Find(data => data.weatherType == weatherType);

        if (soundData != null)
        {
            // 1. 날씨 BGM 설정
            if (soundData.weatherBGM != null && bgmAudioSource != null)
            {
                // 이전 BGM과 다르면 변경
                if (bgmAudioSource.clip != soundData.weatherBGM)
                {
                    bgmAudioSource.Stop();
                    bgmAudioSource.clip = soundData.weatherBGM;
                    bgmAudioSource.Play();
                }
            }

            // 2. 날씨 환경음 설정 (비 소리 등)
            if (soundData.weatherAmbientSFX != null && weatherAmbientSource != null)
            {
                weatherAmbientSource.Stop();
                weatherAmbientSource.clip = soundData.weatherAmbientSFX;
                weatherAmbientSource.volume = soundData.ambientVolume * (isSFXMuted ? 0 : 1);
                weatherAmbientSource.Play();
            }
        }
        else if (weatherType == WeatherType.Sunny)
        {
            // 맑은 날은 기본 BGM으로 복원
            ResetWeatherSound();
        }
    }

    // 기본 BGM으로 복원
    public void ResetWeatherSound()
    {
        // 날씨 환경음 중지
        if (weatherAmbientSource != null)
        {
            weatherAmbientSource.Stop();
            weatherAmbientSource.clip = null;
        }

        currentWeather = WeatherType.Sunny;
        // 현재 씬에 맞는 기본 BGM으로 복원
        PlaySceneBGM(currentSceneName);
    }

    // 씬별 BGM 재생 (볼륨 조절 가능)
    private void PlaySceneBGM(string sceneName)
    {
        var bgmData = sceneBGMs.Find(x => x.sceneName == sceneName);
        if (bgmAudioSource != null && bgmAudioSource.enabled && bgmAudioSource.gameObject.activeInHierarchy)
        {
            bgmAudioSource.Stop();
            bgmAudioSource.clip = bgmData.bgmClip;
            bgmAudioSource.Play();
        }
    }

    private const float MIN_VOLUME_DB = -80f;
    private const float MAX_VOLUME_DB = 0f;

    // 슬라이더용 BGM 볼륨 조절 (0~1 값)
    public void SetBGMVolumeBySlider(float sliderValue)
    {
        if (isBGMMuted)
        {
            return; // 음소거 상태일 때는 볼륨 조절 무시
        }
        float dB = Mathf.Log10(sliderValue) * 20f;
        if (sliderValue <= 0.0001f)
        {
            dB = MIN_VOLUME_DB; // 0에 가까운 값은 음소거로 처리
        }
        audioMixer.SetFloat(bgm, dB);
    }

    // 슬라이더용 효과음 볼륨 조절 (0~1 값)
    public void SetSFXVolumeBySlider(float sliderValue)
    {
        if (isSFXMuted)
        {
            return; // 음소거 상태일 때는 볼륨 조절 무시
        }
        float dB = Mathf.Log10(sliderValue) * 20f;
        if (sliderValue <= 0.0001f)
        {
            dB = MIN_VOLUME_DB; // 0에 가까운 값은 음소거로 처리
        }
        audioMixer.SetFloat(sfx, dB);
    }

    // 외부에서 사운드만 가져다 쓸 수 있게 하는 함수
    public AudioClip GetSFXClip(string sfxName)
    {
        return sfxLibrary?.GetClip(sfxName);
    }

    // 다이얼로그 효과음 재생 (볼륨 조절 가능)
    public void PlaySfxDialog()
    {
        sfxDialogAudioSource.clip = sfxDialog;
        sfxDialogAudioSource.Play();
    }

    public void StopSfxDialog()
    {
        sfxDialogAudioSource.Stop();
    }

    // 효과음 재생 (볼륨 조절 가능)
    public void PlaySFX(string sfxName)
    {
        AudioClip clip = GetSFXClip(sfxName);
        if (clip != null)
        {
            sfxAudioSource.PlayOneShot(clip);
        }
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
            audioMixer.SetFloat(bgm, -80f); // -80dB는 실질적인 음소거
        }
        else
        {
            audioMixer.SetFloat(bgm, PlayerPrefs.GetFloat("BGMVolume")); // 저장된 볼륨으로 복원
        }
    }

    // 효과음 음소거 토글
    public void ToggleSFXMute()
    {
        isSFXMuted = !isSFXMuted;
        if (isSFXMuted)
        {
            audioMixer.SetFloat(sfx, -80f);

            if (weatherAmbientSource != null)
                weatherAmbientSource.volume = 0;
        }
        else
        {
            audioMixer.SetFloat(sfx, PlayerPrefs.GetFloat("SFXVolume")); // 저장된 볼륨으로 복원

            if (weatherAmbientSource != null && currentWeather != WeatherType.Sunny)
            {
                WeatherSoundData soundData = weatherSounds.Find(data => data.weatherType == currentWeather);
                if (soundData != null)
                    weatherAmbientSource.volume = soundData.ambientVolume;
            }
        }
    }

    // 음소거 상태 확인
    public bool IsBGMMuted() => isBGMMuted;
    public bool IsSFXMuted() => isSFXMuted;

}
