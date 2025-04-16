using UnityEngine;

[CreateAssetMenu(fileName = "New Crop", menuName = "Crop/Create New Crop")]
public class CropData : ScriptableObject
{
    public string cropName;
    public Sprite[] growthSprites;
    public int maxGrowthStage;
    public ItemData harvestItem;
}
