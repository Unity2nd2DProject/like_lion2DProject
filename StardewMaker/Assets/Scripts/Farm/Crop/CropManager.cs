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

    public void PlantCrop(Transform parentTtransform, Vector2Int position, CropData cropData, bool _isWatered = false)
    {
        if (crops.ContainsKey(position))
        {
            return;
        }

        //GameObject cropObj = Instantiate(cropPrefabs[cropData.id], new Vector3(position.x, position.y, 0), Quaternion.identity, parentTtransform);
        GameObject cropObj = Instantiate(cropPrefabs[cropData.id], parentTtransform.position, Quaternion.identity, parentTtransform);
        Crop crop = cropObj.GetComponent<Crop>();
        crop.Initialize(cropData, _isWatered);

        crops.Add(position, crop);
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
