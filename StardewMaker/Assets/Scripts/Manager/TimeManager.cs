using System;
using UnityEngine;

public enum Season
{
    Spring,
    Summer,
    Fall,
    Winter
}

public class TimeManager : MonoBehaviour
{
    public static TimeManager Instance;

    [Header("Time Settings")]
    public float realSecondsPerGameDay = 600f; // 10분 = 600초
    private float gameMinutesPerRealSecond;
    private float timer = 0f;
    private bool isTimePaused = false;

    [Header("Date Settings")]
    public int currentYear = 1;
    public Season currentSeason = Season.Spring;
    public int currentDay = 1; // 1~28
    public int currentHour = 7; // AM 07:00 시작
    public int currentMinute = 0;

    public event Action OnDayChanged;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        gameMinutesPerRealSecond = 24f * 60f / realSecondsPerGameDay; // (24시간 * 60분) / 600초
    }

    private void Start()
    {
        UpdateUI();
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

    private void AdvanceDay()
    {
        ResumeTime();
        currentDay++;
        timer = 0f;
        currentHour = 7;
        currentMinute = 0;

        if (currentDay > 28)
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
    }

    public void OnNextDay()
    {
        CropManager.Instance.NextDay();
        FarmLandManager.Instance.NextDay();
        TreeManager.Instance.NextDay();
        StaminaUI.Instance.RecoverStamina(20);
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

    private void UpdateUI()
    {
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
