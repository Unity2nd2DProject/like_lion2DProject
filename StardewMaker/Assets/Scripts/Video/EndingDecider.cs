using UnityEngine;
using System.Collections.Generic;

public class EndingDecider : MonoBehaviour
{
    [Header("엔딩 판정 기준값")]
    [SerializeField] private float highStatThreshold = 80f;    // 높은 수치 기준
    [SerializeField] private float mediumStatThreshold = 70f;  // 중간 수치 기준

    public EndingType DecideEnding()
    {
        List<Stat> currentStats = DaughterManager.Instance.GetStats();
        Dictionary<StatType, float> statValues = new Dictionary<StatType, float>();

        // 스탯 값들을 딕셔너리에 저장
        foreach (var stat in currentStats)
        {
            statValues[stat.statType] = stat.CurrentValue;
            Debug.Log($"현재 {stat.statType} 수치: {stat.CurrentValue}");
        }

        // 엔딩 조건 체크
        EndingType decidedEnding = CheckEndingConditions(statValues);

        // 결정된 엔딩 저장
        PlayerPrefs.SetInt("EndingType", (int)decidedEnding);
        PlayerPrefs.Save();

        Debug.Log($"결정된 엔딩: {decidedEnding}");
        return decidedEnding;
    }

    private EndingType CheckEndingConditions(Dictionary<StatType, float> stats)
    {
        // 예술가 엔딩: 기분과 신뢰도가 높음
        if (stats[StatType.MOOD] >= highStatThreshold && stats[StatType.TRUST] >= highStatThreshold)
            return EndingType.Artist;

        // 모험가 엔딩: 건강과 신뢰도가 높음
        if (stats[StatType.VITALITY] >= highStatThreshold && stats[StatType.TRUST] >= highStatThreshold)
            return EndingType.Adventurer;

        // 요리사 엔딩: 배고픔 관리와 기분이 높음
        if (stats[StatType.HUNGER] >= highStatThreshold && stats[StatType.MOOD] >= highStatThreshold)
            return EndingType.Chef;

        // 선생님 엔딩: 신뢰도가 높고 건강이 중간 이상
        if (stats[StatType.TRUST] >= highStatThreshold && stats[StatType.VITALITY] >= mediumStatThreshold)
            return EndingType.Teacher;

        // 음악가 엔딩: 기분이 높고 건강이 중간 이상
        if (stats[StatType.MOOD] >= highStatThreshold && stats[StatType.VITALITY] >= mediumStatThreshold)
            return EndingType.Musician;

        return EndingType.None;
    }
}