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
    //[SerializeField] private TreeType treeType;
    [SerializeField] private int maxHits = 3;
    [SerializeField] private ItemData woodData;

    [Header("Sprites")]
    [SerializeField] private Sprite normalTreeSprite;
    //[SerializeField] private Sprite fruitTreeSprite;
    [SerializeField] private Sprite stumpSprite;

    [Header("Check")]
    [SerializeField] private int currentHits = 0;
    //[SerializeField] private bool hasFruit = false;

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
    }

    //public void PickFruit()
    //{
    //    if (treeType == TreeType.Fruit && hasFruit)
    //    {
    //        hasFruit = false;
    //        treeType = TreeType.Normal;
    //        UpdateTreeSprite();
    //        // 인벤토리에 과일 추가
    //    }
    //}

    public void NextDay()
    {
        currentHits = 0;
        //gameObject.SetActive(true);

        //if (treeType == TreeType.Fruit)
        //{
        //    hasFruit = true;
        //}

        UpdateTreeSprite();
    }

    //public void SetState(TreeType type, int hits, bool fruitPresent)
    //{
    //    treeType = type;
    //    currentHits = hits;
    //    hasFruit = fruitPresent;
    //    UpdateTreeSprite();
    //}

    public void SetState(int hits)
    {
        currentHits = hits;
        UpdateTreeSprite();
    }

    private void UpdateTreeSprite()
    {
        if (currentHits >= maxHits)
        {
            spriteRenderer.sprite = stumpSprite;
        }
        //else if (treeType == TreeType.Fruit && hasFruit)
        //{
        //    spriteRenderer.sprite = fruitTreeSprite;
        //}
        else
        {
            spriteRenderer.sprite = normalTreeSprite;
        }
    }

    //public (TreeType, int, bool) GetState()
    //{
    //    return (treeType, currentHits, hasFruit);
    //}

    public int GetCurrentHits()
    {
        return currentHits;
    }
}
