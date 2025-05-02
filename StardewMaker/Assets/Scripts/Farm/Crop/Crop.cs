using System;
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
    }

    public void Fertlize()
    {
        currentGrowthStage = cropData.maxGrowthStage;
        isWatered = true;
        UpdateGrowth();
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
        if (cropData.id != 7)
        {
            sr.sprite = cropData.growthSprites[currentGrowthStage];
        }
        else // 전설의 작물
        {
            int lastDay = TimeManager.Instance.LAST_DAY_OF_SEASON;
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
    }

    public bool IsHarvestable()
    {
        return currentGrowthStage == cropData.maxGrowthStage && cropData.id != 7;
    }

    public int GetGrowthStage() => currentGrowthStage;

    public void SetGrowthStage(int stage)
    {
        currentGrowthStage = stage;
        UpdateGrowth();
    }
}
