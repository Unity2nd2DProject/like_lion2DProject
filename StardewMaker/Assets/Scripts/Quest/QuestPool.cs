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
        List<QuestDataSO> candidates = dailyQuests;

        if (candidates.Count == 0)
        {
            return null;
        }

        return candidates[UnityEngine.Random.Range(0, candidates.Count)];
    }
}
