using UnityEngine;
using System.Collections.Generic;


[System.Serializable]
public class StatCondition
{
    public StatType statType;        // 검사할 스탯 유형
    public float requiredValue;      // 해당 스탯의 요구 수치
}


[System.Serializable]
public class EndingCondition
{
    public EndingType endingType;            // 엔딩 유형
    public List<StatCondition> requiredStats; // 이 엔딩에 필요한 스탯 조건들
}


public class EndingDecider : MonoBehaviour
{
    [Header("엔딩 판정 기준값")]
    [SerializeField] private float highStatThreshold = 80f;   // 높은 스탯 기준값
    [SerializeField] private float mediumStatThreshold = 40f; // 중간 스탯 기준값

    [Header("엔딩 조건 설정")]
    [SerializeField] private List<EndingCondition> endingConditions; // 모든 엔딩 조건 목록


    public EndingType DecideEnding()
    {
        // 현재 모든 스탯 정보 가져오기
        List<Stat> currentStats = DaughterManager.Instance.GetStats();
        Dictionary<StatType, float> statValues = new Dictionary<StatType, float>();

        // 스탯 정보를 딕셔너리로 변환하고 로그 출력
        foreach (var stat in currentStats)
        {
            statValues[stat.statType] = stat.CurrentValue;
            Debug.Log($"현재 {stat.statType} 수치: {stat.CurrentValue}");
        }

        return CheckEndingConditions(statValues);
    }


    private EndingType CheckEndingConditions(Dictionary<StatType, float> stats)
    {
        // 모든 엔딩 조건을 순회하며 검사
        foreach (var condition in endingConditions)
        {
            bool allConditionsMet = true;
            // 해당 엔딩에 필요한 모든 스탯 조건 검사
            foreach (var statCondition in condition.requiredStats)
            {
                // 스탯이 존재하지 않거나 요구 수치에 미달인 경우
                if (!stats.ContainsKey(statCondition.statType) ||
                    stats[statCondition.statType] < statCondition.requiredValue)
                {
                    allConditionsMet = false;
                    break;
                }
            }

            // 모든 조건이 만족된 경우
            if (allConditionsMet)
            {
                // 결정된 엔딩 저장 및 반환
                PlayerPrefs.SetInt("EndingType", (int)condition.endingType);
                PlayerPrefs.Save();
                Debug.Log($"결정된 엔딩: {condition.endingType}");
                return condition.endingType;
            }
        }

        // 어떤 조건도 만족하지 못한 경우
        return EndingType.None;
    }
}