using UnityEngine;

public class LegendCrop : Crop
{

    public override void UpdateGrowth()
    {
        int lastDay = TimeManager.Instance.LAST_DAY_OF_SEASON-1;
        if (currentGrowthStage == 0)
        {
            sr.sprite = cropData.growthSprites[0];
        }
        else if (currentGrowthStage > 0 && currentGrowthStage < lastDay)
        {
            sr.sprite = cropData.growthSprites[1];
        }
        else if (currentGrowthStage > lastDay && currentGrowthStage < lastDay * 2)
        {
            sr.sprite = cropData.growthSprites[2];
        }
        else if (currentGrowthStage > lastDay * 2 && currentGrowthStage < lastDay * 3)
        {
            sr.sprite = cropData.growthSprites[3];
        }
        else
        {
            sr.sprite = cropData.growthSprites[4];
        }
    }

    public override bool IsHarvestable()
    {
        return base.IsHarvestable(); // + 딸 스탯 조건 추가
    }
}
