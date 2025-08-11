using UnityEngine;

public enum FruitType
{
    Red,
    Yellow
}

public class Bush : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    [Header("Info")]
    [SerializeField] private ItemData fruitData;
    [SerializeField] private FruitType fruitType;

    [Header("Sprites")]
    [SerializeField] private Sprite withFruitSprite;
    [SerializeField] private Sprite withoutFruitSprite;

    [Header("Check")]
    [SerializeField] private bool hasFruit = false;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        UpdateSprite();
    }

    public void SetState(FruitType type, bool _hasFruit)
    {
        fruitType = type;
        hasFruit = _hasFruit;
        UpdateSprite();
    }


    public void PickFruit()
    {
        if (hasFruit)
        {
            hasFruit = false;
            UpdateSprite();
            InventoryManager.Instance.AddItem(fruitData, 3);
        }
    }

    private void UpdateSprite()
    {
        if (hasFruit)
        {
            spriteRenderer.sprite = withFruitSprite;
        }
        else
        {
            spriteRenderer.sprite = withoutFruitSprite;
        }
    }

    public void NextDay()
    {
        hasFruit = true;
        UpdateSprite();
    }

    public (FruitType, bool) GetState()
    {
        return (fruitType, hasFruit);
    }
    
    public bool CanPick()
    {
        return hasFruit;
    }
}
