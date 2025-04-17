using UnityEngine;

public class Crop : MonoBehaviour
{
    public CropData cropData;
    private int currentGrowthStage = 0;
    public SpriteRenderer currentSprite;

    public bool isWatered = false;

    private void Awake()
    {
        currentSprite = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        
    }

    public void Water()
    {
        isWatered = true;
        Debug.Log($"{cropData.name} is watered!");
    }

    public void NextDay()
    {
        if (isWatered)
        {
            if (currentGrowthStage < cropData.maxGrowthStage)
            {
                currentGrowthStage++;
                UpdateGrowth();
            }
        }

        isWatered = false;
    }

    private void UpdateGrowth()
    {
        currentSprite.sprite = cropData.growthSprites[currentGrowthStage];
        Debug.Log($"{cropData.name} is growed!");
    }

    public void Harvest()
    {
        if (Inventory.Instance.AddItem(cropData.harvestItem, 1))
        {
            Destroy(gameObject);
        }
    }

    public bool IsHarvestable()
    {
        return currentGrowthStage == cropData.maxGrowthStage;
    }
}
