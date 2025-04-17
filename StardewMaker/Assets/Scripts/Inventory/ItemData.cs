using UnityEngine;

public enum ItemType
{
    Seed,
    Crop,
    Food,
    Tool,
    Etc
}

[CreateAssetMenu(fileName = "New Item", menuName = "Item/Create New Item")]
public class ItemData : ScriptableObject
{
    public string itemName;
    public ItemType itemType;
    public Sprite icon;
    public bool isStackable; // Tool -> false
    public CropData cropToGrow; // only Seed
}
