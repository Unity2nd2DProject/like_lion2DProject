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

    public List<SavedCrop> NextDayCrops()
    {
        List<SavedCrop> list = new List<SavedCrop>();
        foreach (var kv in crops)
        {
            var crop = kv.Value;
            var nextGrowthStage = crop.GetGrowthStage();
            if (crop.isWatered)
            {
                nextGrowthStage = Mathf.Max(crop.GetGrowthStage() + 1, crop.cropData.maxGrowthStage);
            }

            list.Add(new SavedCrop
            {
                position = kv.Key,
                cropId = crop.cropData.id,
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

}
