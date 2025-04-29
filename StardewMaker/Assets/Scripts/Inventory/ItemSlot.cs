using UnityEngine;
using UnityEngine.EventSystems;

public class ItemSlot
{
    public ItemData itemData;
    public int quantity;

    public bool IsEmpty()
    {
        return itemData == null || quantity <= 0;
    }

    public void Clear()
    {
        itemData = null;
        quantity = 0;
    }    
}