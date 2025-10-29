using UnityEngine;

[CreateAssetMenu(fileName = "New Crop", menuName = "Crop/Create New Crop")]
public class CropData : ScriptableObject
{
    public int id;
    public string cropName;
    public int growthInterval = 120; // 분 단위
    public Sprite[] growthSprites;
    public int maxGrowthStage;
    public int harvestNum;
    public ItemData harvestItem;
}
