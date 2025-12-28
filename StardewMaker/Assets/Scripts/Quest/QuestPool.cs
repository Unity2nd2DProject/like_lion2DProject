using System.Collections.Generic;
using System;
using UnityEngine;
using NPC;

[CreateAssetMenu(fileName = "Quest Pool", menuName = "Quest/Create Quest Pool")]
public class QuestPool : ScriptableObject
{
    public NpcId npcId;
    public List<QuestDataSO> dailyQuests;

    public QuestDataSO GetRandomAvailableQuest()
    {
        List<QuestDataSO> candidates = new List<QuestDataSO>(dailyQuests);

        if (candidates.Count == 0)
        {
            return null;
        }

        return candidates[UnityEngine.Random.Range(0, candidates.Count)];
    }
}
