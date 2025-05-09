using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

// 날씨 유형 열거형
public enum WeatherType
{
    Sunny,      // 화창한 날
    Rainy,      // 비 오는 날
    Cloudy,     // 흐린 날
    Snowy       // 눈 오는 날
}

[System.Serializable]
public class WeatherDate
{
    public Season season;     // 계절
    public int day;           // 날짜
    public WeatherType weatherType; // 날씨 유형
}

public class WeatherManager : Singleton<WeatherManager>
{


    [Header("날씨 효과")]
    [SerializeField] private GameObject rainPrefab;      // 비 파티클 프리팹
    [SerializeField] private GameObject cloudyPrefab;    // 흐린 날 효과 프리팹
    [SerializeField] private GameObject snowPrefab;      // 눈 파티클 프리팹

    [Header("날씨 효과 부모")]
    [SerializeField] private Transform weatherEffectsParent; // 날씨 효과들이 생성될 부모 오브젝트

    // 현재 활성화된 날씨 효과 오브젝트
    private GameObject currentWeatherEffect;

    [Header("날씨 설정")]
    [SerializeField] private List<WeatherDate> weatherDates = new List<WeatherDate>();
    [SerializeField] private float rainyDarknessIntensity = 0.3f; // 비 올 때 어두워지는 정도
    [SerializeField] private float cloudyDarknessIntensity = 0.7f; // 비 올 때 어두워지는 정도
    [SerializeField] private float snowyBrightnessBoost = 1.2f;   // 눈 올 때 밝아지는 정도

    // 현재 날씨
    private WeatherType currentWeather = WeatherType.Sunny;
    private Color originalLightColor;

    private bool isSubscribedToSceneLoad = false;

    protected override void Awake()
    {
        base.Awake();

        if (!isValid) return;
        // 파티클 효과 초기화 (모두 비활성화)
        DestroyCurrentWeatherEffect();

        // 씬 로드 이벤트 구독
        SceneManager.sceneLoaded += OnSceneLoaded;
        isSubscribedToSceneLoad = true;
    }

    private void Start()
    {
        // 초기 날씨 설정 (GameMode 확인 후 적용)
        UpdateWeather();

        // TimeManager의 날짜 변경 이벤트에 구독
        if (TimeManager.Instance != null)
        {
            TimeManager.Instance.OnDayChanged += UpdateWeather;
        }
    }

    private void OnDestroy()
    {
        // 이벤트 구독 해제
        if (TimeManager.Instance != null)
        {
            TimeManager.Instance.OnDayChanged -= UpdateWeather;
        }

        // 씬 로드 이벤트 구독 해제
        if (isSubscribedToSceneLoad)
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }

    // 씬 로드 이벤트 핸들러 추가
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // 씬 전환 후 현재 GameMode에 따라 날씨 효과 업데이트
        ApplyCurrentWeather();

    }

    // TOWN 모드 체크
    private bool IsTownMode()
    {
        return GameManager.Instance != null && GameManager.Instance.currentMode == GameMode.TOWN;
    }

    public void UpdateWeather()
    {
        if (TimeManager.Instance == null) return;

        Season currentSeason = TimeManager.Instance.currentSeason;
        int currentDay = TimeManager.Instance.currentDay;

        // 현재 날짜와 일치하는 날씨 데이터 찾기
        WeatherType newWeather = WeatherType.Sunny; // 기본값은 맑음

        foreach (WeatherDate date in weatherDates)
        {
            if (date.season == currentSeason && date.day == currentDay)
            {
                newWeather = date.weatherType;
                break;
            }
        }

        // 날씨가 변경되었을 경우에만 업데이트
        if (currentWeather != newWeather)
        {
            ChangeWeather(newWeather);
        }
        else
        {
            // 날씨는 같지만 모드가 바뀌었을 수 있으므로 효과 적용 확인
            ApplyCurrentWeather();
        }
    }

    // 현재 날씨 효과 적용 (모드 확인 후)
    private void ApplyCurrentWeather()
    {
        // 기존 효과 제거
        DestroyCurrentWeatherEffect();

        // TOWN 모드일 때만 날씨 효과 적용
        if (IsTownMode())
        {
            ApplyWeatherEffects();
        }
        else
        {
            // HOME 모드에서는 모든 효과 비활성화
            DisableAllWeatherEffects();
        }
    }

    // 날씨 효과 적용
    private void ApplyWeatherEffects()
    {
        // 날씨에 따른 파티클 및 조명 효과 활성화
        switch (currentWeather)
        {
            case WeatherType.Rainy:
                InstantiateWeatherEffect(rainPrefab);
                AdjustLightingForRain();
                break;

            case WeatherType.Cloudy:
                InstantiateWeatherEffect(cloudyPrefab);
                AdjustLightingForCloudy();
                break;

            case WeatherType.Snowy:
                InstantiateWeatherEffect(snowPrefab);
                AdjustLightingForSnow();
                break;

            default: // Sunny
                ResetLighting();
                break;
        }

        // 날씨 사운드 업데이트
        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.UpdateWeatherSound(currentWeather);
        }
    }

    // 모든 날씨 효과 비활성화
    private void DisableAllWeatherEffects()
    {
        // 파티클 효과 제거
        DestroyCurrentWeatherEffect();

        // 조명 효과 초기화
        ResetLighting();

        // 날씨 사운드 초기화 (기본 BGM으로 복원)
        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.ResetWeatherSound();
        }
    }

    private void ChangeWeather(WeatherType weatherType)
    {
        // 이전 날씨 효과 비활성화
        DestroyCurrentWeatherEffect();

        // 새 날씨 설정
        currentWeather = weatherType;

        // 현재 모드에 따라 날씨 효과 적용
        ApplyCurrentWeather();

        Debug.Log($"날씨 변경: {weatherType}, Town 모드: {IsTownMode()}");
    }

    private void InstantiateWeatherEffect(GameObject prefab)
    {
        if (prefab == null) return;

        // 부모 오브젝트가 없으면 찾아보고 없으면 리턴
        if (weatherEffectsParent == null)
        {
            GameObject effectObj = GameObject.Find("Effect");
            if (effectObj == null)
                return;
        }

        // 날씨 효과 생성하고 부모 아래에 배치
        currentWeatherEffect = Instantiate(prefab, Vector3.zero, Quaternion.identity);
        currentWeatherEffect.transform.SetParent(weatherEffectsParent, false);
    }
    private void DestroyCurrentWeatherEffect()
    {
        if (currentWeatherEffect != null)
        {
            Destroy(currentWeatherEffect);
            currentWeatherEffect = null;
        }
    }

    private void AdjustLightingForRain()
    {
        if (LightManager.Instance != null)
        {
            // LightManager의 날씨 계수만 설정
            LightManager.Instance.weatherMultiplier = rainyDarknessIntensity;
        }
    }

    private void AdjustLightingForCloudy()
    {
        if (LightManager.Instance != null)
        {
            LightManager.Instance.weatherMultiplier = 0.85f;
        }
    }

    private void AdjustLightingForSnow()
    {
        if (LightManager.Instance != null)
        {
            LightManager.Instance.weatherMultiplier = snowyBrightnessBoost;
        }
    }

    private void ResetLighting()
    {
        // 원래 라이팅으로 복원
        if (LightManager.Instance != null)
        {
            // 맑은 날은 기본값
            LightManager.Instance.weatherMultiplier = 1.0f;
        }
    }

    // 현재 날씨 반환 (다른 시스템에서 체크용)
    public WeatherType GetCurrentWeather()
    {
        return currentWeather;
    }

    // 수동으로 날씨 변경 (디버그/테스트용)
    public void SetWeather(WeatherType weatherType)
    {
        ChangeWeather(weatherType);
    }

    // 수동으로 날씨 효과 강제 적용 (TOWN 모드가 아닐 때도 적용 가능)
    public void ForceApplyWeatherEffects()
    {
        ApplyWeatherEffects();
    }
}
