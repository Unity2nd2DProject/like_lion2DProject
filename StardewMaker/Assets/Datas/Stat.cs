using System;
using UnityEngine;

public enum StatType
{
    NONE,
    MOOD,
    VITALITY,
    HUNGER,
    TRUST,

    PYSICAL,
    MUSIC,
    ART,
    SOCIAL,
    ACADEMIC,
    DOMESTIC,

}

public static class StatTypeExtensions
{
    public static string ToKorean(this StatType statType)
    {
        switch (statType)
        {
            case StatType.MOOD:
                return "기분";
            case StatType.VITALITY:
                return "건강";
            case StatType.HUNGER:
                return "배고픔";
            case StatType.TRUST:
                return "신뢰도";
            case StatType.PYSICAL:
                return "운동";
            case StatType.MUSIC:
                return "음악";
            case StatType.ART:
                return "미술";
            case StatType.SOCIAL:
                return "사교";
            case StatType.ACADEMIC:
                return "학문";
            case StatType.DOMESTIC:
                return "생활";
            default:
                return statType.ToString();
        }
    }
}

[CreateAssetMenu(menuName = "Stats/Stat", fileName = "New Stat")]
public class Stat : ScriptableObject
{
    public StatType statType;
    public float currentValue;
    public float maxValue;

    public float CurrentValue
    {
        get { return currentValue; }
        set
        {
            if (currentValue != value)
            {
                currentValue = value;
                OnValueChanged?.Invoke(currentValue); // 값이 바뀌었을 때만 호출
            }
        }
    }
    public float MaxValue
    {
        get { return maxValue; }
    }

    public void Initialize(StatType type, float max)
    {
        statType = type;
        maxValue = max;
        currentValue = max;
    }
    public void Initialize(StatType type, float max, float current)
    {
        statType = type;
        maxValue = max;
        currentValue = current;
    }

    public event Action<float> OnValueChanged;

    public float GetNormalizedValue()
    {
        return currentValue / maxValue;
    }
}
