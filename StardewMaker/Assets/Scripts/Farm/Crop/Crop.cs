using System;
using UnityEngine;

public class Crop : MonoBehaviour
{
    public CropData cropData;
    public int currentGrowthStage = 0;
    public bool isWatered = false;

    protected SpriteRenderer sr;

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

    public virtual void Fertlize()
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

    public virtual void UpdateGrowth()
    {
        sr.sprite = cropData.growthSprites[currentGrowthStage];
    }

    public virtual bool IsHarvestable()
    {
        return currentGrowthStage == cropData.maxGrowthStage;
    }

    public int GetGrowthStage() => currentGrowthStage;

    public void SetGrowthStage(int stage)
    {
        currentGrowthStage = stage;
        UpdateGrowth();
    }
}
