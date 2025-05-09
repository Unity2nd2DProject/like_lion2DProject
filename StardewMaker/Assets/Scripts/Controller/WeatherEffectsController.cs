using UnityEngine;
using UnityEngine.SceneManagement;


public class WeatherEffectsController : MonoBehaviour
{
    [Header("날씨 효과 프리팹")]
    [SerializeField] private GameObject rainPrefab;
    [SerializeField] private GameObject cloudyPrefab;
    [SerializeField] private GameObject snowPrefab;

    [Header("효과 설정")]
    [SerializeField] private Transform effectsParent;
    [SerializeField] private bool enableParticleEffects = true;
    [SerializeField] private bool enableSoundEffects = true;
    [SerializeField] private bool enableLightingEffects = true;

    [Header("모드 설정")]
    [SerializeField] private bool onlyInTownMode = true;

    [Header("조명 설정")]
    [SerializeField] private float rainyDarknessIntensity = 0.3f;
    [SerializeField] private float cloudyDarknessIntensity = 0.7f;
    [SerializeField] private float snowyBrightnessBoost = 1.2f;

    // 현재 활성화된 날씨 효과 오브젝트
    private GameObject currentWeatherEffect;

    // 현재 적용 중인 날씨
    private WeatherType currentAppliedWeather = WeatherType.Sunny;

    private void Awake()
    {
        // 효과 부모 오브젝트 확인
        if (effectsParent == null)
        {
            effectsParent = transform;
        }
    }

    private void OnEnable()
    {
        // 날씨 데이터 매니저 이벤트 구독
        if (WeatherManager.Instance != null)
        {
            WeatherManager.Instance.OnWeatherChanged += OnWeatherChanged;
        }

        // 씬 로드 이벤트 구독
        SceneManager.sceneLoaded += OnSceneLoaded;

        // 현재 날씨에 맞게 효과 적용
        ApplyCurrentWeatherEffects();
    }

    private void OnDisable()
    {
        // 이벤트 구독 해제
        if (WeatherManager.Instance != null)
        {
            WeatherManager.Instance.OnWeatherChanged -= OnWeatherChanged;
        }

        SceneManager.sceneLoaded -= OnSceneLoaded;

        // 효과 정리
        DestroyCurrentWeatherEffect();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // 씬 전환 시 효과 업데이트
        ApplyCurrentWeatherEffects();
    }

    // 날씨 변경 이벤트 핸들러
    private void OnWeatherChanged(WeatherType newWeather)
    {
        currentAppliedWeather = newWeather;
        ApplyCurrentWeatherEffects();
    }

    // 현재 모드가 TOWN인지 확인
    private bool IsTownMode()
    {
        return GameManager.Instance != null && GameManager.Instance.currentMode == GameMode.TOWN;
    }

    // 현재 날씨 효과를 적용해야 하는지 확인
    private bool ShouldApplyEffects()
    {
        // Town 모드 제한이 활성화된 경우
        if (onlyInTownMode && !IsTownMode())
        {
            return false;
        }

        return true;
    }

    // 현재 날씨에 맞게 효과 적용
    public void ApplyCurrentWeatherEffects()
    {
        // 이전 효과 제거
        DestroyCurrentWeatherEffect();

        // 적용 여부 확인
        if (!ShouldApplyEffects())
        {
            // 효과가 적용되지 않을 경우 기본 상태로 복원
            ResetEffects();
            return;
        }

        // 현재 날씨 가져오기
        WeatherType currentWeather = currentAppliedWeather;
        if (WeatherManager.Instance != null)
        {
            currentWeather = WeatherManager.Instance.GetCurrentWeather();
        }

        // 날씨별 효과 적용
        switch (currentWeather)
        {
            case WeatherType.Rainy:
                if (enableParticleEffects) InstantiateWeatherEffect(rainPrefab);
                if (enableLightingEffects) AdjustLightingForRain();
                break;

            case WeatherType.Cloudy:
                if (enableParticleEffects) InstantiateWeatherEffect(cloudyPrefab);
                if (enableLightingEffects) AdjustLightingForCloudy();
                break;

            case WeatherType.Snowy:
                if (enableParticleEffects) InstantiateWeatherEffect(snowPrefab);
                if (enableLightingEffects) AdjustLightingForSnow();
                break;

            default: // Sunny
                if (enableLightingEffects) ResetLighting();
                break;
        }

        // 사운드 업데이트
        if (enableSoundEffects && SoundManager.Instance != null)
        {
            SoundManager.Instance.UpdateWeatherSound(currentWeather);
        }
    }

    // 모든 효과 리셋
    private void ResetEffects()
    {
        DestroyCurrentWeatherEffect();
        ResetLighting();

        // 사운드 리셋 (기본 BGM으로 복원)
        if (enableSoundEffects && SoundManager.Instance != null)
        {
            SoundManager.Instance.ResetWeatherSound();
        }
    }

    // 날씨 효과 인스턴스화
    private void InstantiateWeatherEffect(GameObject prefab)
    {
        if (prefab == null) return;

        // 효과 생성 및 부모 설정
        currentWeatherEffect = Instantiate(prefab, Vector3.zero, Quaternion.identity);
        currentWeatherEffect.transform.SetParent(effectsParent, false);
    }

    // 현재 효과 제거
    private void DestroyCurrentWeatherEffect()
    {
        if (currentWeatherEffect != null)
        {
            Destroy(currentWeatherEffect);
            currentWeatherEffect = null;
        }
    }

    // 조명 관련 메서드들
    private void AdjustLightingForRain()
    {
        if (LightManager.Instance != null)
        {
            LightManager.Instance.weatherMultiplier = rainyDarknessIntensity;
        }
    }

    private void AdjustLightingForCloudy()
    {
        if (LightManager.Instance != null)
        {
            LightManager.Instance.weatherMultiplier = cloudyDarknessIntensity;
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
        if (LightManager.Instance != null)
        {
            LightManager.Instance.weatherMultiplier = 1.0f;
        }
    }

    // 외부에서 각 효과 활성화/비활성화
    public void SetParticleEffectsEnabled(bool enabled)
    {
        enableParticleEffects = enabled;
        ApplyCurrentWeatherEffects();
    }

    public void SetSoundEffectsEnabled(bool enabled)
    {
        enableSoundEffects = enabled;
        ApplyCurrentWeatherEffects();
    }

    public void SetLightingEffectsEnabled(bool enabled)
    {
        enableLightingEffects = enabled;
        ApplyCurrentWeatherEffects();
    }
}