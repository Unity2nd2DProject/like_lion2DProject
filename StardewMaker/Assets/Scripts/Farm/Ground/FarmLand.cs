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
    public Crop crop;

    private Vector2 position;

    private SpriteRenderer sr;
    //[SerializeField] private SpriteRenderer cropSr;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        //cropSr = GetComponentInChildren<SpriteRenderer>();

        position = transform.position;
        UpdateTileSprite();
    }

    public bool Pick()
    {
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

    public bool Plant(Crop _crop)
    {
        if (crop == null && landState == LandState.Fertile)
        {
            CropManager.Instance.crops.Add(_crop);
            crop = _crop;
            Debug.Log($"Succed to plant {crop}! {position}");
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool Water()
    {
        if (crop != null && landState == LandState.Fertile)
        {
            crop.Water();
            UpdateTileSprite();
            Debug.Log($"Succed to water {crop}! {position}");
            return true;
        }
        else
        {
            return false;
        }
    }

    private void UpdateTileSprite()
    {
        switch (landState)
        {
            case LandState.Normal:
                sr.sprite = normalSprite; // null
                break;
            case LandState.Fertile:
                if (crop != null && crop.isWatered)
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
