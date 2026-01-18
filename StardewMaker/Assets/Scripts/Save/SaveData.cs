using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    public FarmSaveData farmSaveData = new FarmSaveData();
    public InventoryData inventorySaveData = new InventoryData();
}

#region FarmData
[System.Serializable]
public class FarmSaveData
{
    public List<SavedFarmLand> savedFarmLands = new List<SavedFarmLand>();
    public List<SavedCrop> savedCrops = new List<SavedCrop>();
    public List<SavedTree> savedTrees = new List<SavedTree>();
    public List<SavedBush> savedBushes = new List<SavedBush>();
}

[System.Serializable]
public class SavedTree
{
    public Vector2 position;
    public int currentHits;
    public int daysSinceCut;
}

[System.Serializable]
public class SavedBush
{
    public Vector2 position;
    public bool hasFruit;
    public FruitType fruitType;
}

[System.Serializable]
public class SavedFarmLand
{
    public Vector2 position;
    public FamrLandState landState;
}

[System.Serializable]
public class SavedCrop
{
    public Vector2 position;
    public int cropId;
    public int currentGrowthStage;
    public int fertlizerCount;
    public bool isWatered;
    public int timesSinceWater;
}

#endregion

public class BaseSaveData
{
    public SavedTimeData savedTimeData;
    public SavedPlayerData savedPlayerData;
}
// GameBaseData
[System.Serializable]
public class SavedTimeData
{
    // Date and Time
    public int year;
    public int season;
    public int day;
    public int hour;
    public int minute;
}

public class SavedPlayerData
{
    public Vector3 position;

    public StaminaState[] staminaStates;
    public int money;
}

// InventoryData
public class InventoryData
{
    public List<savedInventroyItem> savedInventory = new List<savedInventroyItem>();
}

[System.Serializable]
public class savedInventroyItem
{
    public int slotIndex; // 0~24 : Inventory, 25 ~ 34 :Quickslot
    public ItemData itemData;
    public int quantity;
}

// 태그

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

// NPCData
[System.Serializable]
public class NPCSaveData
{
    public List<SavedNPC> savedNPCs = new List<SavedNPC>();
}

[System.Serializable]
public class SavedNPC
{
    public NPC.NpcId npcId;
    public Vector3 position;
    public NpcActionType currentAction;
    public int routeIndex;
    public string teleportTarget;
}

[System.Serializable]
public class FriendshipSaveData
{
    public List<SavedFriendship> savedFriendships = new List<SavedFriendship>();
}

[System.Serializable]
public class SavedFriendship
{
    public NPC.NpcId npcId;
    public int points;
}

// QuestData
[System.Serializable]
public class SavedQuestData
{
    public List<SavedQuest> activeQuests = new List<SavedQuest>();
    public List<string> completedQuestIDs = new List<string>();
}

[System.Serializable]
public class SavedQuest
{
    public string questID;
    public NPC.NpcId giverNpcId;
    public List<int> currentAmounts = new List<int>();
}

