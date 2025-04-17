using UnityEngine;

public enum LandState
{
    Normal,
    Fertile,
    Watered
}

public class LandData : MonoBehaviour
{
    public LandState landState;
    public Sprite normalSprite;
    public Sprite fertileSprite;

    private SpriteRenderer sr;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        UpdateTileSprite();
    }

    public void Pick()
    {
        if (landState == LandState.Normal)
        {
            landState = LandState.Fertile;
            UpdateTileSprite();
        }
        else if (landState == LandState.Fertile)
        {
            landState = LandState.Normal;
            UpdateTileSprite();
        }
    }

    public void Water()
    {
        if (landState == LandState.Fertile)
        {
            landState = LandState.Watered;
            UpdateTileSprite();
        }
    }

    private void UpdateTileSprite()
    {
        switch (landState)
        {
            case LandState.Normal:
                sr.sprite = normalSprite; // 나중에 null로
                break;
            case LandState.Fertile:
                sr.sprite = fertileSprite;
                break;
            case LandState.Watered:
                sr.sprite = fertileSprite;
                sr.color = new Color(0.7f, 0.5f, 0.3f);
                break;
        }
    }
}
