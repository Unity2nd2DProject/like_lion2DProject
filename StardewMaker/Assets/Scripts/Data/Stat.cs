using System;
using UnityEngine;



public enum StatType
{
    Mood,
    Vitality,
    Hunger,
    Trust
}

public static class StatTypeExtensions
{
    public static string ToKorean(this StatType statType)
    {
        switch (statType)
        {
            case StatType.Mood:
                return "기분";
            case StatType.Vitality:
                return "건강";
            case StatType.Hunger:
                return "배고픔";
            case StatType.Trust:
                return "신뢰도";
            default:
                return statType.ToString();
        }
    }
}

public class Stat
{
    public StatType statType;
    private float currentValue;
    private float maxValue;

    public float CurrentValue
    {
        get { return currentValue; }
        set
        {
            OnValueChanged.Invoke(currentValue); // Notify the change
        }
    }
    public float MaxValue
    {
        get { return maxValue; }
    }

    public Stat(StatType type, float max)
    {
        statType = type;
        maxValue = max;
        currentValue = max;
    }
    public Stat(StatType type, float max, float current)
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
