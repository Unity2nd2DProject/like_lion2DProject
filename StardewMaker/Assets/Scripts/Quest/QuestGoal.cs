using UnityEditor;
using UnityEngine;

public enum QuestGoalType
{
    Action,    
    ItemCollect 
}

public enum QuestTargetType
{
    TrilledSoil,
    SeedPlanted,
    Watered,
    Fertilized,
    Harvested,
    TreeChopped,
    FishCaught,
    StoneBroken,
    GaveToDaughter,
    CookedFood,
    GreetedToNPC,
    BuyItem,
}

[System.Serializable]
public class QuestGoal
{
    public QuestGoalType goalType;

    public QuestTargetType targetType; 
    public ItemData targetItem;      

    public int requiredAmount;
}
