using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SoundSettingUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI bgmText;
    [SerializeField] private TextMeshProUGUI sfxText;
    [SerializeField] private Slider bgmSlider;
    [SerializeField] private Slider sfxSlider;
    [SerializeField] private Button bgmMuteButton;
    [SerializeField] private Button sfxMuteButton;
    [SerializeField] private Image bgmMuteIcon;
    [SerializeField] private Image sfxMuteIcon;

    [Header("UI Control")]
    [SerializeField] private GameObject soundSettingPanel;    // 사운드 설정 패널 -> 사실상 this.gameObject와 동일. 
    [SerializeField] private Button openButton;               // 설정 열기 버튼
    [SerializeField] private Button closeButton;              // 설정 닫기 버튼

    private void Start()
    {
        // PlayerPrefs에서 저장된 값 불러오기 (없으면 기본값 0.5f 사용)
        bgmSlider.value = PlayerPrefs.GetFloat("BGMVolume", 0.5f);
        sfxSlider.value = PlayerPrefs.GetFloat("SFXVolume", 0.5f);

        // 초기 텍스트 설정
        UpdateBGMText(bgmSlider.value);
        UpdateSFXText(sfxSlider.value);

        // 슬라이더 이벤트 연결
        bgmSlider.onValueChanged.AddListener(OnBGMVolumeChanged);
        sfxSlider.onValueChanged.AddListener(OnSFXVolumeChanged);

        // 음소거 버튼 이벤트 연결
        bgmMuteButton.onClick.AddListener(OnBGMMuteButtonClicked);
        sfxMuteButton.onClick.AddListener(OnSFXMuteButtonClicked);

        // 초기 아이콘 상태 설정
        UpdateMuteIcons();

        // 초기 상태에서는 음소거 아이콘 숨기기
        bgmMuteIcon.enabled = false;
        sfxMuteIcon.enabled = false;

        // 설정 UI 버튼 이벤트 연결
        if (openButton != null)
            openButton.onClick.AddListener(OpenSoundSetting);
        if (closeButton != null)
            closeButton.onClick.AddListener(CloseSoundSetting);

        /*
        // 초기에는 설정 패널 비활성화
        if (soundSettingPanel != null)
            soundSettingPanel.SetActive(false);
        */
    }

    private void OnBGMVolumeChanged(float value)
    {
        SoundManager.Instance.SetBGMVolumeBySlider(value);
        Debug.Log("BGM Volume Changed: " + value);
        UpdateBGMText(value);
        PlayerPrefs.SetFloat("BGMVolume", value);
        PlayerPrefs.Save();
    }

    private void OnSFXVolumeChanged(float value)
    {
        SoundManager.Instance.SetSFXVolumeBySlider(value);
        UpdateSFXText(value);
        PlayerPrefs.SetFloat("SFXVolume", value);
        PlayerPrefs.Save();
    }

    private void OnBGMMuteButtonClicked()
    {
        SoundManager.Instance.ToggleBGMMute();
        UpdateMuteIcons();
    }

    private void OnSFXMuteButtonClicked()
    {
        SoundManager.Instance.ToggleSFXMute();
        UpdateMuteIcons();
    }

    private void UpdateBGMText(float value)
    {
        bgmText.text = $"BGM: {(value * 100):F0}%";
    }

    private void UpdateSFXText(float value)
    {
        sfxText.text = $"SFX: {(value * 100):F0}%";
    }

    private void UpdateMuteIcons()
    {
        // 음소거 상태일 때만 아이콘 표시
        bgmMuteIcon.enabled = SoundManager.Instance.IsBGMMuted();
        sfxMuteIcon.enabled = SoundManager.Instance.IsSFXMuted();
    }

    public void OpenSoundSetting()
    {
        if (soundSettingPanel != null)
        {
            soundSettingPanel.SetActive(true);
            // 설정창이 열릴 때 현재 저장된 값으로 UI 업데이트
            UpdateBGMText(bgmSlider.value);
            UpdateSFXText(sfxSlider.value);
            UpdateMuteIcons();
        }
    }

    public void CloseSoundSetting()
    {
        if (soundSettingPanel != null)
        {
            soundSettingPanel.SetActive(false);
            // 설정값 저장
            PlayerPrefs.Save(); // 필요한가?
        }
    }

    /*
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && soundSettingPanel.activeSelf)
        {
            CloseSoundSetting();
        }
    }
    */
}