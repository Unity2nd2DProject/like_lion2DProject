using UnityEngine;

[System.Serializable]
public class QuestGoal
{
    public QuestTargetType targetType;
    public int requiredAmount;
    [HideInInspector] public int currentAmount;

    public bool IsComplete => currentAmount >= requiredAmount;

    public void Report()
    {
        currentAmount++;
    }
}
