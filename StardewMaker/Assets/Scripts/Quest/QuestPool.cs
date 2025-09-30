using System.Collections.Generic;
using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Quest Pool", menuName = "Quest/Create Quest Pool")]
public class QuestPool : ScriptableObject
{
    public string npcName;
    public List<QuestDataSO> dailyQuests;

    public QuestDataSO GetRandomAvailableQuest(int currentHour, int day)
    {
        List<QuestDataSO> candidates = dailyQuests.FindAll(q =>
            currentHour >= q.availableFromHour &&
            currentHour < q.availableToHour &&
            (q.availableDays == null || q.availableDays.Count == 0 || q.availableDays.Contains(day))
        );

        if (candidates.Count == 0)
        {
            return null;
        }

        return candidates[UnityEngine.Random.Range(0, candidates.Count)];
    }
}
