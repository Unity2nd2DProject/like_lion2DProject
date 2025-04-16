using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory Instance;

    public int inventorySize = 50;
    public List<InventorySlot> slots = new List<InventorySlot>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

    }

    public bool AddItem(ItemData item, int amount = 1)
    {
        if (item.isStackable)
        {
            foreach (var slot in slots)
            {
                if (slot.itemData == item)
                {
                    slot.quantity += amount;
                    return true;
                }
            }
        }
        else
        {
            foreach(var slot in slots)
            {
                if (slot.IsEmpty())
                {
                    slot.itemData = item;
                    slot.quantity = amount;
                    return true;
                }
            }
        }

        Debug.Log("Inventory is full..");
        return false;
    }

    public void RemoveItem(ItemData item, int amount = 1)
    {
        foreach (var slot in slots)
        {
            if (slot.itemData == item)
            {
                slot.quantity -= amount;

                if (slot.quantity <= 0)
                {
                    slot.Clear();
                    break;
                }
            }
        }
    }
}
