using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class QuestInstance
{
    public QuestDataSO questData;
    public NPCID giverNpcName;
    public List<QuestGoal> goals = new();

    public bool IsComplete => goals.TrueForAll(g => g.IsComplete);

    public QuestInstance(QuestDataSO data, NPCID giverName)
    {
        questData = data;
        giverNpcName = giverName;
        foreach (var goal in data.goals)
        {
            goals.Add(new QuestGoal
            {
                goalType = goal.goalType,
                targetType = goal.targetType,
                targetItem = goal.targetItem,
                requiredAmount = goal.requiredAmount,
                currentAmount = 0
            });
        }
    }
}
