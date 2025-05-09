using System;
using System.Collections.Generic;
using UnityEngine;

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
    [Header("날씨 설정")]
    [SerializeField] private List<WeatherDate> weatherDates = new List<WeatherDate>();

    // 현재 날씨
    private WeatherType currentWeather = WeatherType.Sunny;

    // 날씨 변경 이벤트 (다른 시스템에서 구독 가능)
    public event Action<WeatherType> OnWeatherChanged;

    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        // 초기 날씨 설정
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

        // 날씨가 변경되었을 경우에만 이벤트 발생
        if (currentWeather != newWeather)
        {
            ChangeWeather(newWeather);
        }
    }

    private void ChangeWeather(WeatherType weatherType)
    {
        currentWeather = weatherType;
        Debug.Log($"날씨 변경: {weatherType}");

        // 날씨 변경 이벤트 발생
        OnWeatherChanged?.Invoke(weatherType);
    }

    // 현재 날씨 반환
    public WeatherType GetCurrentWeather()
    {
        return currentWeather;
    }


    // 현재 날짜 기반으로 날씨 데이터 찾기 (애디터 용)
    public WeatherDate GetWeatherData(Season season, int day)
    {
        return weatherDates.Find(date => date.season == season && date.day == day);
    }


}