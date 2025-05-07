using System;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

public enum Season
{
    Spring,
    Summer,
    Fall,
    Winter
}

public class TimeManager : Singleton<TimeManager>
{
    public int LAST_DAY_OF_SEASON = 7;
    public int START_HOUR = 7;

    [Header("Time Settings")]
    public float realSecondsPerGameDay = 600f; // 10분 = 600초
    private float gameMinutesPerRealSecond;
    private float timer = 0f;
    private bool isTimePaused = false;

    [Header("Date Settings")]
    public int currentYear = 1;
    public Season currentSeason = Season.Spring;
    public int currentDay = 1; // 1~7
    public int currentHour = 7; // AM 07:00 시작
    public int currentMinute = 0;

    protected override void Awake()
    {
        base.Awake();
        if (!isValid) return; // 없어질 게임오브젝트면 아래 명령들 실행 안 함

        gameMinutesPerRealSecond = 24f * 60f / realSecondsPerGameDay; // (24시간 * 60분) / 600초

        Debug.Log("TimeManager Awake");
        CheckCurrentScene(); // 홈씬에서 시작하는 경우 시간 멈추기
    }

    private void CheckCurrentScene()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        if (currentScene.name.Contains("HomeScene")) PauseTime();
    }

    private void Start()
    {
        SaveManager.Instance.LoadTime();
    }

    private void Update()
    {
        if (isTimePaused)
        {
            return;
        }

        timer += Time.deltaTime * gameMinutesPerRealSecond;

        if (timer >= 1f)
        {
            AdvanceMinute();
            timer -= 1f;
        }

        ForceReturnHome();

        if (!isTimePaused)
        {
            LightManager.Instance.UpdateLighting(currentHour, currentMinute);
        }
    }

    private void AdvanceMinute()
    {
        currentMinute++;
        if (currentMinute >= 60)
        {
            currentMinute = 0;
            currentHour++;

            if (currentHour >= 24)
            {
                currentHour = 0;
                AdvanceDay();
            }
        }

        if (currentMinute % 10 == 0)
        {
            UpdateUI();
        }
    }

    public void AdvanceDay()
    {
        ResumeTime();
        currentDay++;
        timer = 0f;
        currentHour = START_HOUR;
        currentMinute = 0;

        if (currentDay > LAST_DAY_OF_SEASON)
        {
            currentDay = 1;
            currentSeason++;

            if ((int)currentSeason > 3) // 겨울 끝나면
            {
                currentSeason = Season.Spring;
                currentYear++;
            }
        }

        OnNextDay();

        UpdateUI(); // 업데이트 한 번 해 줌
        CheckCurrentScene(); // 홈씬이면 시간 멈춤
    }

    public void OnNextDay()
    {
        CropManager.Instance.NextDay();
        FarmLandManager.Instance.NextDay();
        TreeManager.Instance.NextDay();
        StaminaManager.Instance.RecoverStamina(20);
        UpdateUI();
    }

    private void ForceReturnHome()
    {
        if (currentHour >= 22)
        {
            Debug.Log("🌙 시간이 늦었습니다 집으로 귀가합니다");
            PauseTime();

            Debug.Log("☀️ 아침이 되었습니다 하루를 시작합니다");
            AdvanceDay();
        }
    }

    public void PauseTime() // 집에 들어가면
    {
        isTimePaused = true;
    }

    public void ResumeTime() // 집에서 나오면
    {
        isTimePaused = false;
    }

    public void UpdateUI()
    {
        //Debug.Log($"TimeUI Update : {currentHour} : {currentMinute}");
        if (currentHour >= 7 && currentHour < 18)
        {
            TimeImageUI.Instance.SetDayImage();
        }
        else
        {
            TimeImageUI.Instance.SetNightImage();
        }

        BaseUI.Instance.SetDateText(GetCurrentDateString());
        BaseUI.Instance.SetTimeText(GetCurrentTimeString());
    }

    private string GetCurrentDateString()
    {
        return $"{GetSeasonString()} {currentDay}일";
    }

    private string GetSeasonString()
    {
        switch (currentSeason)
        {
            case Season.Spring: return "봄";
            case Season.Summer: return "여름";
            case Season.Fall: return "가을";
            case Season.Winter: return "겨울";
            default: return "봄";
        }
    }

    private string GetCurrentTimeString()
    {
        return $"{GetAmPmTimeString()}";
    }

    private string GetAmPmTimeString()
    {
        string period = (currentHour < 12) ? "AM" : "PM";
        int displayHour = currentHour % 12;
        if (displayHour == 0)
        {
            displayHour = 12;
        }

        return $"{period} {displayHour:D2}:{currentMinute:D2}";
    }
}
