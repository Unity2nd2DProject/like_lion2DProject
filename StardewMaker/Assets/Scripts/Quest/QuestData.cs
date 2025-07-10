using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Quest", menuName = "Quest/Create New Quest")]
public class QuestData : ScriptableObject
{
    [Header("Info")]
    public string questID;
    public string questName;
    [TextArea] public string description;
    public string goalDescrpition;

    [Header("Goal")]
    public List<QuestGoal> goals = new List<QuestGoal>();

    [Header("Reward")]
    public ItemData rewardItem;
    public int rewardQuantity;
}
