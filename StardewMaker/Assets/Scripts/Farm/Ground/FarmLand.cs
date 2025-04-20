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

    private Vector2Int position; // connect with crop

    private SpriteRenderer sr;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();

        position = new Vector2Int(
            Mathf.RoundToInt(transform.position.x),
            Mathf.RoundToInt(transform.position.y)
        );

        //UpdateTileSprite();
    }

    public bool Plant(CropData cropData)
    {
        if (landState != LandState.Normal)
        {
            if (landState == LandState.Watered)
            {
                CropManager.Instance.PlantCrop(position, cropData, true);
            }
            else
            {
                CropManager.Instance.PlantCrop(position, cropData);
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
        if (CropManager.Instance.GetCropAt(position) != null)
        {
            return false;
        }

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

    public bool Water()
    {
        if (landState == LandState.Fertile)
        {
            landState = LandState.Watered;
            CropManager.Instance.WaterCrop(position);
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
        if (CropManager.Instance.GetCropAt(position).IsHarvestable())
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

    private void UpdateTileSprite()
    {
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

    public Vector2Int GetPosition()
    {
        return position;
    }
}
