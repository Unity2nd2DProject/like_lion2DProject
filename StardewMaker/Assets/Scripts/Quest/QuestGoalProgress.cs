using System;

[Serializable]
public class QuestGoalProgress
{
    public QuestGoalType goalType;
    public QuestTargetType targetType;
    public ItemData targetItem;

    public int requiredAmount;
    public int currentAmount;

    public bool IsComplete => currentAmount >= requiredAmount;

    public void Report() => currentAmount++;
}
