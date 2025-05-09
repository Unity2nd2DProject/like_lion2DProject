using UnityEngine;

public class LegendCrop : Crop
{
    public int fertilizerCount = 0;

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
        else if (currentGrowthStage >= 15 && currentGrowthStage < 21) 
        {
            sr.sprite = cropData.growthSprites[3];
        }
        else if (currentGrowthStage >= 21 && IsHarvestable())
        {
            sr.sprite = cropData.growthSprites[4];
        }
    }

    public override void Fertlize()
    {
        fertilizerCount += 1;
        Debug.Log("fertilCount : " + fertilizerCount);
    }

    public int GetFertilizerCount()
    {
        return fertilizerCount;
    }

    public override bool IsHarvestable()
    {
        return base.IsHarvestable() && TimeManager.Instance.IsLastDay(); 
    }

    public void SetFertilizerCount(int count)
    {
        fertilizerCount = count;
    }
}
