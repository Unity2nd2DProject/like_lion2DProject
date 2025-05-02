using UnityEngine;

public class LegendCrop : Crop
{

    public override void UpdateGrowth()
    {
        if (currentGrowthStage <= 0)
        {
            sr.sprite = cropData.growthSprites[0];
        }
        else if (currentGrowthStage > 0 && currentGrowthStage <= 7) 
        {
            sr.sprite = cropData.growthSprites[1];
        }
        else if (currentGrowthStage > 7 && currentGrowthStage < 15) 
        {
            sr.sprite = cropData.growthSprites[2];
        }
        else if (currentGrowthStage >= 15 && currentGrowthStage < 20) 
        {
            sr.sprite = cropData.growthSprites[3];
        }
        else if (currentGrowthStage >= 20 && IsHarvestable())
        {
            sr.sprite = cropData.growthSprites[4];
        }
    }

    public override bool IsHarvestable()
    {
        return base.IsHarvestable(); // + 딸 스탯 조건 추가, 게임 일수 추가
    }
}
