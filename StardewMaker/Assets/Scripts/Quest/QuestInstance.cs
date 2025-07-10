using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class QuestInstance
{
    public QuestData questData;
    public List<QuestGoal> goals = new();

    public bool IsComplete => goals.TrueForAll(g => g.IsComplete);

    public QuestInstance(QuestData data)
    {
        questData = data;
        foreach (var goal in data.goals)
        {
            goals.Add(new QuestGoal
            {
                targetType = goal.targetType,
                requiredAmount = goal.requiredAmount,
                currentAmount = 0
            });
        }
    }
}
