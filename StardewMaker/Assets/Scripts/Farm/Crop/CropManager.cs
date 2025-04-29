using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CropManager : Singleton<CropManager>
{
    //public static CropManager Instance;

    public GameObject[] cropPrefabs;
    public Dictionary<Vector2, Crop> crops = new Dictionary<Vector2, Crop>();

    protected override void Awake()
    {
        base.Awake();
    }

    //private void Awake()
    //{
    //    if (Instance == null)
    //    {
    //        Instance = this;
    //    }
    //}

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
}
