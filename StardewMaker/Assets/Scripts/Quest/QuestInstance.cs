using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class QuestInstance
{
    public QuestDataSO questData;
    public NPC.NpcId giverNpcId;
    public List<QuestGoalProgress> goals = new();

    public bool IsComplete => goals.TrueForAll(g => g.IsComplete);

    public QuestInstance(QuestDataSO data, NPC.NpcId giverID)
    {
        questData = data;
        giverNpcId = giverID;

        foreach (var goal in data.goals)
        {
            goals.Add(new QuestGoalProgress
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
