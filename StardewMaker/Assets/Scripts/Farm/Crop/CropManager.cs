using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class CropManager : MonoBehaviour
{
    public static CropManager Instance;

    public GameObject[] cropPrefabs;
    public Dictionary<Vector2Int, Crop> crops = new Dictionary<Vector2Int, Crop>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public void PlantCrop(Vector2Int gridPos, CropData cropData)
    {
        if (crops.ContainsKey(gridPos))
        {
            return;
        }

        Vector3 worldPos = new Vector3(gridPos.x, gridPos.y, 0);
        
        GameObject cropObj = Instantiate(cropPrefabs[cropData.id], worldPos, Quaternion.identity);
        Crop crop = cropObj.GetComponent<Crop>();
        crop.Initialize(cropData);

        crops.Add(gridPos, crop);
    }

    public void WaterCrop(Vector2Int gridPos)
    {
        if (crops.TryGetValue(gridPos, out Crop crop))
        {
            crop.Water();
        }
    }

    public void HarvestCrop(Vector2Int gridPos)
    {
        if (crops.TryGetValue(gridPos, out Crop crop))
        {
            if (crop.IsHarvestable())
            {
                Destroy(crop.gameObject);
                crops.Remove(gridPos);
                Inventory.Instance.AddItem(crop.cropData.harvestItem);
            }
        }
    }

    public void RemoveCrop(Vector2Int gridPos)
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

    public Crop GetCropAt(Vector2Int gridPos)
    {
        crops.TryGetValue(gridPos, out Crop crop);
        return crop;
    }

    public Dictionary<Vector2Int, Crop> GetAllCrops()
    {
        return crops;
    }
}
