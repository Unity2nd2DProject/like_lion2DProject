using NUnit.Framework.Interfaces;
using UnityEngine;

public enum LandState
{
    Normal,
    Fertile,
    Watered
}

public class FarmLand : MonoBehaviour
{
    public LandState landState;
    public Sprite normalSprite;
    public Sprite fertileSprite;
    public Sprite wateredSprite;

    private Vector2 position; // connect with crop

    private SpriteRenderer sr;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();

        position = new Vector2(transform.position.x, transform.position.y);

        UpdateTileSprite();
    }

    public bool Plant(ItemData itemData)
    {
        if (landState != LandState.Normal)
        {
            if (landState == LandState.Watered)
            {
                CropManager.Instance.PlantCrop(transform, position, itemData.cropToGrow, true);
            }
            else
            {
                CropManager.Instance.PlantCrop(transform, position, itemData.cropToGrow);
            }

            if (!InventoryManager.Instance.RemoveItem(itemData))
            {
                InventoryManager.Instance.RemoveItem(itemData);
            }

            return true;
        }
        else
        {
            return false;
        }
    }

    public bool Pick()
    {
        if (CanPick())
        {
            if (landState == LandState.Normal)
            {
                landState = LandState.Fertile;
                UpdateTileSprite();
                return true;
            }
            else
            {
                landState = LandState.Normal;
                UpdateTileSprite();
                return true;
            }
        }
        else
        {
            return false;
        }
    }

    public bool Water()
    {
        if (CanWater())
        {
            landState = LandState.Watered;
            if (CropManager.Instance.GetCropAt(position) != null)
            {
                CropManager.Instance.WaterCrop(position);
            }
            InventoryManager.Instance.RemoveItem(InventoryManager.Instance.GetItem("물"));

            UpdateTileSprite();
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool Harvest()
    {
        if (CanHarvest())
        {
            CropManager.Instance.HarvestCrop(position);
            UpdateTileSprite();
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool Fertilize()
    {
        if (CanFertilze())
        {
            CropManager.Instance.FertilizeCrop(position);

            InventoryManager.Instance.RemoveItem(InventoryManager.Instance.GetItem("비료"));

            UpdateTileSprite();
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool Remvoe()
    {
        return true;
    }

    public void NextDay()
    {
        if (landState == LandState.Watered)
        {
            landState = LandState.Fertile;
        }

        UpdateTileSprite();
    }

    public void UpdateTileSprite()
    {
        //Debug.Log("UpdateTileSprite");
        switch (landState)
        {
            case LandState.Normal:
                sr.sprite = normalSprite;
                break;
            case LandState.Fertile:
                sr.sprite = fertileSprite;
                break;
            case LandState.Watered:
                sr.sprite = wateredSprite;
                break;
        }
    }

    public Vector2 GetPosition()
    {
        return position;
    }

    public bool CanPlant(ItemData itemData)
    {
        return landState != LandState.Normal &&
               itemData != null &&
               itemData.cropToGrow != null &&
               CropManager.Instance.GetCropAt(position) == null;
    }

    public bool CanPick()
    {
        return CropManager.Instance.GetCropAt(position) == null;
    }

    public bool CanWater()
    {
        return landState == LandState.Fertile &&
               InventoryManager.Instance.GetItem("물") != null;
    }

    public bool CanHarvest()
    {
        var crop = CropManager.Instance.GetCropAt(position);
        return crop != null && crop.IsHarvestable();
    }

    public bool CanFertilze()
    {
        var crop = CropManager.Instance.GetCropAt(position);
        return crop != null && !crop.IsHarvestable() &&
            InventoryManager.Instance.GetItem("비료") != null;
    }
}
