using UnityEngine;

public enum ItemType
{
    Seed,
    Ingredient,
    Food,
    Tool,
    Etc
}

[CreateAssetMenu(fileName = "New Item", menuName = "Item/Create New Item")]
public class ItemData : ScriptableObject
{
    [Header("Base Info")]
    public string itemName;
    public ItemType itemType;
    public Sprite icon;
    public string itemDescription;
    public bool isStackable; // Tool -> false
    public CropData cropToGrow; // only Seed
    [Header("Shop Info")]
    public int buyPrice;
    public int sellPrice;
    public bool isSellable; // Tool -> false
}
