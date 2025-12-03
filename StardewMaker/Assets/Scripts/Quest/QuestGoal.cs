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
    [HideInInspector] public int currentAmount;

    public bool IsComplete => currentAmount >= requiredAmount;

    public void Report()
    {
        currentAmount++;
    }

    public string GetDescription()
    {
        return goalType switch
        {
            QuestGoalType.Action => $"{targetType}: {currentAmount}/{requiredAmount}",
            QuestGoalType.ItemCollect => $"{targetItem.itemName}: {currentAmount}/{requiredAmount}",
            _ => ""
        };
    }
}
