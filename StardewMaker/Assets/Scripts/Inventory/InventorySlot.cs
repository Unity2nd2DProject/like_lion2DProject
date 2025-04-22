using UnityEngine;
using UnityEngine.EventSystems;

public class InventorySlot
{
    public ItemData itemData;
    public int quantity;

    public bool IsEmpty()
    {
        return itemData == null;
    }

    public void Clear()
    {
        itemData = null;
        quantity = 0;
    }    
}