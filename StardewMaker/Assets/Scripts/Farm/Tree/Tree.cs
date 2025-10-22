using UnityEngine;

//public enum TreeType
//{
//    Normal,
//    Fruit
//}

public class Tree : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    [Header("Info")]
    [SerializeField] private int maxHits = 3;
    [SerializeField] private int growDay = 3;
    [SerializeField] private ItemData woodData;

    [Header("Sprites")]
    [SerializeField] private Sprite normalTreeSprite;
    [SerializeField] private Sprite stumpSprite;

    [Header("Check")]
    [SerializeField] private int currentHits = 0;
    [SerializeField] private int daysSinceCut = 0;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        UpdateTreeSprite();
    }

    public void Chop()
    {
        currentHits++;
        InventoryManager.Instance.AddItem(woodData, 1);
        QuestManager.Instance.ReportAction(QuestTargetType.TreeChopped);
        PlayerManager.Instance.AddExpToSkill(TraitType.Woodcutting);

        if (currentHits >= maxHits)
        {
            BecomeStump();
        }
    }

    private void BecomeStump()
    {
        spriteRenderer.sprite = stumpSprite;
        daysSinceCut = 0;
    }

    public void NextDay()
    {
        if (currentHits >= maxHits)
        {
            daysSinceCut++;

            if (daysSinceCut >= growDay)
            {
                RegrowTree();
            }
        }

        UpdateTreeSprite();
    }

    private void RegrowTree()
    {
        currentHits = 0;
        daysSinceCut = 0;
        spriteRenderer.sprite = normalTreeSprite;
    }

    public void SetState(int hits, int _daysSinceCut)
    {
        currentHits = hits;
        daysSinceCut = _daysSinceCut;
        UpdateTreeSprite();
    }

    private void UpdateTreeSprite()
    {
        if (currentHits >= maxHits)
        {
            spriteRenderer.sprite = stumpSprite;
        }
        else
        {
            spriteRenderer.sprite = normalTreeSprite;
        }
    }

    public bool CanChop()
    {
        if (currentHits >= maxHits)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    public int GetCurrentHits() => currentHits;
    public int GetDaysSinceCut() => daysSinceCut;
}
