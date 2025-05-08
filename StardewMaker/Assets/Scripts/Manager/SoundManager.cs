using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;  // SceneManager 사용을 위해 추가
using System.Collections.Generic;
using System;
using UnityEngine.Rendering;

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

    private string bgm = "BGM";
    private string sfx = "SFX";

    private bool isBGMMuted = false;
    private bool isSFXMuted = false;

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
        SetVolume();
        PlaySceneBGM(scene.name);
    }

    private void SetVolume()
    {
        audioMixer.SetFloat(bgm, PlayerPrefs.GetFloat("BGMVolume")); // 저장된 BGM 볼륨으로 복원
        audioMixer.SetFloat(sfx, PlayerPrefs.GetFloat("SFXVolume")); // 저장된 SFX 볼륨으로 복원

        if (isBGMMuted)
        {
            audioMixer.SetFloat(bgm, -80f); // 음소거
        }
        if (isSFXMuted)
        {
            audioMixer.SetFloat(sfx, -80f); // 음소거
        }           
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
        }
        else
        {
            audioMixer.SetFloat(sfx, PlayerPrefs.GetFloat("SFXVolume")); // 저장된 볼륨으로 복원
        }
    }

    // 음소거 상태 확인
    public bool IsBGMMuted() => isBGMMuted;
    public bool IsSFXMuted() => isSFXMuted;

}
