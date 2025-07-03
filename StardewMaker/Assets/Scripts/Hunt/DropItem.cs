using UnityEngine;

[System.Serializable]
public class DropItem
{
    public ItemData itemData;
    public int quantity = 1;
    public float dropChance = 1f;

    public DropItem(ItemData itemData, int quantity = 1, float dropChance = 1f)
    {
        this.itemData = itemData;
        this.quantity = quantity;
        this.dropChance = dropChance;
    }
}
