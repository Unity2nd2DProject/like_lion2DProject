using System;
using System.Collections.Generic;
using UnityEngine;

public enum QuestType
{
    Tutorial,
    Quest
}

[CreateAssetMenu(fileName = "New Quest", menuName = "Quest/Create New Quest")]
public class QuestData : ScriptableObject
{
    [Header("Info")]
    public string questID;
    public QuestType questType;
    public string questName;
    [TextArea] public string description;
    public string goalDescrpition;

    [Header("Goal")]
    public List<QuestGoal> goals = new List<QuestGoal>();

    [Header("Reward")]
    public ItemData rewardItem;
    public int rewardQuantity;
    public int rewardMoney;
    public string npcName;
    public int friendshipPointReward;

    [Header("Condition")]
    public int availableFromHour;
    public int availableToHour;
    public List<int> availableDays;
}
