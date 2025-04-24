using NUnit.Framework.Interfaces;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : Singleton<Inventory>
{
    public int inventorySize = 25;
    public List<ItemSlot> slots = new List<ItemSlot>();

    public List<ItemData> starterItems = new List<ItemData>(); // 테스트를 위해 시작 아이템 추가 

    protected override void Awake()
    {
        base.Awake();

        InitInventory();
    }

    private void InitInventory()
    {
        for (int i = 0; i < inventorySize; i++)
        {
            slots.Add(new ItemSlot()); // 슬롯 초기화
        }

        // 테스트 용 아이템 추가
        for (int i = 0; i < starterItems.Count; i++)
        {
            AddItem(starterItems[i], 4);
        }
        InventoryUI.Instance.UpdateInventoryUI();
    }

    public bool AddItem(ItemData newItem, int amount = 1)
    {
        if (newItem.isStackable)
        {
            foreach (var inventorySlot in slots)
            {
                if (inventorySlot.itemData == newItem) // 같은 아이템이 있는 슬롯을 찾음
                {
                    inventorySlot.quantity += amount; // 수량 증가
                    InventoryUI.Instance.UpdateInventoryUI();
                    return true; // 아이템 추가 완료
                }
            }
            foreach (var inventorySlot in slots)
            {
                if (inventorySlot.IsEmpty()) // 비어있는 슬롯을 찾음
                {
                    inventorySlot.itemData = newItem; // 아이템 할당
                    inventorySlot.quantity = amount; // 수량 설정
                    InventoryUI.Instance.UpdateInventoryUI();
                    return true; // 아이템 추가 완료
                }
            }      
        }
        else
        {
            foreach (var inventorySlot in slots)
            {
                if (inventorySlot.IsEmpty()) // 비어있는 슬롯을 찾음
                {
                    inventorySlot.itemData = newItem; // 아이템 할당
                    inventorySlot.quantity = amount; // 수량 설정
                    InventoryUI.Instance.UpdateInventoryUI();
                    return true; // 아이템 추가 완료
                }
            }            
        }
        return false; // 슬롯이 부족하여 아이템 추가 실패
    }

    public bool RemoveItem(ItemData item, int amount = 1)
    {
        foreach (var slot in slots)
        {
            if (slot.itemData == item)
            {
                if (slot.quantity >= amount)
                {
                    slot.quantity -= amount;

                    if (slot.quantity <= 0) // 수량이 0 이하가 되면 슬롯을 비움
                    {
                        slot.itemData = null;
                        slot.quantity = 0;
                    }
                    InventoryUI.Instance.UpdateInventoryUI();
                    return true; // 아이템 제거 성공
                }
            }
        }

        return false; // 아이템이 없거나 수량 부족
    }

    public ItemData GetItem(string itemName)
    {
        foreach (var slot in slots)
        {
            if (!slot.IsEmpty() && slot.itemData.itemName == itemName)
            {
                return slot.itemData;
            }
        }

        return null;
    }

}
