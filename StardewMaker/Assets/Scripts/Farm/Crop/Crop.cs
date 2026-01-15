using System;
using UnityEngine;

public class Crop : MonoBehaviour
{
    public CropData cropData;
    public int currentGrowthStage = 0;
    public bool isWatered = false;
    public int timesSinceWater = 0;
    private Vector2 position;

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
        if (isWatered)
        {
            return;
        }

        isWatered = true;
        timesSinceWater = 0;
    }

    public virtual void Fertlize()
    {
        currentGrowthStage = cropData.maxGrowthStage;
        isWatered = true;
        UpdateGrowth();
    }

    public void OnTimeChanged()
    {
        if (isWatered)
        {
            timesSinceWater++;

            if (timesSinceWater >= cropData.growthInterval)
            {
                Grow();
                timesSinceWater = 0;
            }
        }
    }

    public void Grow()
    {
        if (isWatered)
        {
            if (currentGrowthStage < cropData.maxGrowthStage)
            {
                currentGrowthStage++;
                UpdateGrowth();
                FarmLand land = FarmingManager.Instance.GetFarmLandAt(position);
                Debug.Log($"land : {land}");
                if (land != null)
                {
                    land.AbsorbAwater();
                }

                isWatered = false;
            }

        }
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

    public void SetData(int stage, int _timeSinceWater)
    {
        currentGrowthStage = stage;
        timesSinceWater = _timeSinceWater;
        UpdateGrowth();
    }

    public void SetPosition(Vector2 pos)
    {
        position = pos;
    }
}
