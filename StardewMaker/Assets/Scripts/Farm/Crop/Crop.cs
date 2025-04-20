using UnityEngine;

public class Crop : MonoBehaviour
{
    public CropData cropData;
    private int currentGrowthStage = 0;
    public bool isWatered = false;

    private SpriteRenderer sr;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    public void Initialize(CropData data, bool _isWatered = false)
    {
        cropData = data;
        isWatered = _isWatered;
        UpdateGrowth();
    }

    public void Water()
    {
        isWatered = true;
        Debug.Log($"💧 {cropData.name} get water!");
    }

    public void NextDay()
    {
        if (isWatered)
        {
            if (currentGrowthStage < cropData.maxGrowthStage)
            {
                currentGrowthStage++;
                UpdateGrowth();
            }
        }
        isWatered = false;
    }

    private void UpdateGrowth()
    {
        sr.sprite = cropData.growthSprites[currentGrowthStage];
    }

    public bool IsHarvestable()
    {
        return currentGrowthStage == cropData.maxGrowthStage;
    }
}
