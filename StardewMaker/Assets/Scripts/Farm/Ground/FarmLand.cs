using UnityEngine;

public enum LandState
{
    Normal,
    Fertile
}

public class FarmLand : MonoBehaviour
{
    public LandState landState;
    public Sprite normalSprite;
    public Sprite fertileSprite;

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
        if (landState == LandState.Fertile)
        {
            CropManager.Instance.PlantCrop(position, cropData);
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
        else if (landState == LandState.Fertile)
        {
            landState = LandState.Normal;
            UpdateTileSprite();
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool Water()
    {
        if (CropManager.Instance.GetCropAt(position) != null)
        {
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

    private void UpdateTileSprite()
    {
        switch (landState)
        {
            case LandState.Normal:
                sr.sprite = normalSprite; // null
                break;
            case LandState.Fertile:
                if (CropManager.Instance.GetCropAt(position) != null && CropManager.Instance.GetCropAt(position).isWatered)
                {
                    sr.sprite = fertileSprite;
                    sr.color = new Color(0.7f, 0.5f, 0.3f);
                }
                else
                {
                    sr.sprite = fertileSprite;
                }
                break;
        }
    }
}
