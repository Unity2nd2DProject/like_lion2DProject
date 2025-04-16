using UnityEngine;

public class Crop : MonoBehaviour
{
    public CropData cropData;
    private int currentGrowthStage = 0;
    private bool isWatered = false;
    private SpriteRenderer currentSprite;

    private void Start()
    {
        
    }

    public void Water()
    {
        isWatered = true;
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
        currentSprite.sprite = cropData.growthSprites[currentGrowthStage];
    }

    public bool IsHarvestable()
    {
        return currentGrowthStage == cropData.maxGrowthStage;
    }
}
