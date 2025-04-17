using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory Instance;

    public int inventorySize = 25;
    public List<InventorySlot> slots = new List<InventorySlot>();
    public List<ItemData> starterItems = new List<ItemData>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        InitInventory();
    }

    private void InitInventory()
    {
        for(int i = 0; i < inventorySize; i++)
        {
            slots.Add(new InventorySlot());
        }

        //for(int i=0; i<starterItems.Count; i++) // Test
        //{
        //    AddItem(starterItems[i]);
        //}

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
                    Debug.Log($"{item.name} is added to inventory! (+{amount})");
                    return true;
                }
            }

            foreach (var slot in slots)
            {
                if (slot.IsEmpty())
                {
                    slot.itemData = item;
                    slot.quantity = amount;
                    Debug.Log($"{item.name} is added to inventory! (+{amount})");
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
                    Debug.Log($"{item.name}({amount}) is added to inventory!");
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
