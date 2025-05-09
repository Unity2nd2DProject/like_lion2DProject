using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class FarmData
{
    public List<SavedFarmLand> savedFarmLands = new List<SavedFarmLand>();
    public List<SavedCrop> savedCrops = new List<SavedCrop>();
    public List<SavedTree> savedTrees = new List<SavedTree>();
}

[System.Serializable]
public class SavedTree
{
    public Vector2 position;
    public int currentHits;
    public bool isActive;
}

[System.Serializable]
public class SavedFarmLand
{
    public Vector2 position;
    public LandState landState;
}

[System.Serializable]
public class SavedCrop
{
    public Vector2 position;
    public int cropId;
    public int currentGrowthStage;
    public int fertlizerCount;
    public bool isWatered;
}

[System.Serializable]
public class GameBaseData
{
    public int year;
    public int season;
    public int day;
    public int hour;
    public int minute;

    public StaminaState[] staminaStates;
    public int money;
}

public class InventoryData
{
    public List<savedInventroyItem> savedInventory = new List<savedInventroyItem>();
}

[System.Serializable]
public class savedInventroyItem
{
    public int slotIndex; // 0~24 : Inventory, 25 ~ 34 :Quickslot
    public string itemName;
    public int quantity;
}

[System.Serializable]
public class StatsData
{
    public List<SavedStat> savedStats = new List<SavedStat>();
}


[System.Serializable]
public class SavedStat
{
    public StatType statType;
    public float currentValue;
    public float maxValue;
}
