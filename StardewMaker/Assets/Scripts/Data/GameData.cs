using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
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
    public bool isWatered;
}
