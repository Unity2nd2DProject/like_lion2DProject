using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CropManager : Singleton<CropManager>
{
    public GameObject[] cropPrefabs;
    public List<CropData> cropDatabase;
    public Dictionary<Vector2, Crop> crops = new Dictionary<Vector2, Crop>();

    protected override void Awake()
    {
        base.Awake();
    }
    public void PlantCrop(Transform parentTtransform, Vector2 position, CropData cropData, bool _isWatered = false)
    {
        if (crops.ContainsKey(position))
        {
            return;
        }

        GameObject cropObj = Instantiate(cropPrefabs[cropData.id], parentTtransform.position, Quaternion.identity, parentTtransform);

        Crop crop = cropObj.GetComponent<Crop>();
        crop.Initialize(cropData, _isWatered);

        crops.Add(position, crop);
    }

    public void WaterCrop(Vector2 gridPos)
    {
        if (crops.TryGetValue(gridPos, out Crop crop))
        {
            crop.Water();
        }
    }

    public void HarvestCrop(Vector2 gridPos)
    {
        if (crops.TryGetValue(gridPos, out Crop crop))
        {
            if (crop.IsHarvestable())
            {
                Destroy(crop.gameObject);
                crops.Remove(gridPos);
                InventoryManager.Instance.AddItem(crop.cropData.harvestItem, crop.cropData.harvestNum);
            }
        }
    }

    public void FertilizeCrop(Vector2 gridPos)
    {
        if (crops.TryGetValue(gridPos, out Crop crop))
        {
            crop.Fertlize();
        }
    }

    public void RemoveCrop(Vector2 gridPos)
    {
        if (crops.TryGetValue(gridPos, out Crop crop))
        {
            Destroy(crop.gameObject);
            crops.Remove(gridPos);
        }
    }

    public void NextDay()
    {
        foreach (var crop in crops.Values)
        {
            crop.NextDay();
        }
    }

    public Crop GetCropAt(Vector2 gridPos)
    {
        crops.TryGetValue(gridPos, out Crop crop);
        return crop;
    }

    public Dictionary<Vector2, Crop> GetAllCrops()
    {
        return crops;
    }

    public List<SavedCrop> SaveCrops()
    {
        List<SavedCrop> list = new List<SavedCrop>();
        foreach (var kv in crops)
        {
            var crop = kv.Value;
            list.Add(new SavedCrop
            {
                position = kv.Key,
                cropId = crop.cropData.id,
                currentGrowthStage = crop.GetGrowthStage(),
                isWatered = crop.isWatered
            });
        }
        return list;
    }

    public List<SavedCrop> NextDayCrops(List<SavedCrop> savedList)
    {
        List<SavedCrop> list = new List<SavedCrop>();

        foreach (var saved in savedList)
        {
            var nextGrowthStage = saved.currentGrowthStage;
            var maxGrowthStage = cropDatabase.Find(c => c.id == saved.cropId).maxGrowthStage;
            if (saved.isWatered)
            {
                nextGrowthStage = Mathf.Min(nextGrowthStage + 1, maxGrowthStage);
            }

            list.Add(new SavedCrop
            {
                position = saved.position,
                cropId = saved.cropId,
                currentGrowthStage = nextGrowthStage,
                isWatered = false
            });
        }
        return list;
    }

    public void LoadCrops(List<SavedCrop> savedList)
    {
        foreach (var saved in savedList)
        {
            var cropData = cropDatabase.Find(c => c.id == saved.cropId);
            if (cropData == null) continue;

            GameObject obj = Instantiate(cropPrefabs[saved.cropId], saved.position, Quaternion.identity);
            Crop crop = obj.GetComponent<Crop>();
            crop.Initialize(cropData, saved.isWatered);
            crop.SetGrowthStage(saved.currentGrowthStage);

            crops[saved.position] = crop;
        }
    }

    public void RegisterCrop(Vector2 position, Crop crop)
    {
        crops[position] = crop;
    }

    public EndingResult GetEndingResult()
    {
        foreach (var crop in crops)
        {
            if (crop.Value.cropData.id == 7)
            {
                LegendCrop legendCrop = (LegendCrop)crop.Value;
                int maxCount = 27;
                int growthCount = legendCrop.GetGrowthStage();
                int fertilizerCount = legendCrop.GetFertilizerCount();

                float waterRatio = growthCount / (float)maxCount;
                float fertilizerRatio = fertilizerCount / (float)maxCount;

                if (waterRatio >= 0.8f && fertilizerRatio >= 0.3f)
                {
                    return EndingResult.GOOD;
                }
                else if (waterRatio >= 0.5f && fertilizerRatio >= 0f && fertilizerRatio < 0.3f)
                {
                    return EndingResult.NORMAL;
                }
                else if (waterRatio < 0.5f && fertilizerRatio == 0f)
                {
                    return EndingResult.BAD;
                }
            }
        }

        return EndingResult.BAD;
    }

    public void GotoLastDay(EndingResult ending) // Debug
    {
        foreach (var crop in crops)
        {
            if (crop.Value.cropData.id == 7)
            {
                switch(ending)
                {
                    case EndingResult.GOOD:
                        crop.Value.currentGrowthStage = 25;
                        break;
                    case EndingResult.NORMAL:
                        break;
                    case EndingResult.BAD:
                        break;
                }

                crop.Value.UpdateGrowth();
            }
        }
    }
}

public enum EndingResult
{
    GOOD,
    NORMAL,
    BAD
}
