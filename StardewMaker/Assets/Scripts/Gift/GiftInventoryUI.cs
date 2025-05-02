using UnityEngine;
using System.Collections.Generic;
using System;

public class GiftInventoryUI : MonoBehaviour
{
    public GiftSlotUI[] giftSlots; // 선물 슬롯 UI 배열

    private void Start()
    {
    }

    public void InitializeGiftInventoryUI()
    {
        UpdateGiftInventory(); // 선물 인벤토리 업데이트
    }

    public void UpdateGiftInventory()
    {
        int giftInventoryIndex = 0;
        InventoryManager inventoryManager = InventoryManager.Instance;

        for (int i = 0; i < inventoryManager.inventorySize + inventoryManager.quickSlotSize; i++)
        {
            if (inventoryManager.slots[i].IsEmpty() || inventoryManager.slots[i].itemData.itemType == ItemType.Seed || inventoryManager.slots[i].itemData.itemType == ItemType.Tool)
            {
                continue; // 빈 슬롯이거나 씨앗, 도구인 경우 건너뜀                
            }
            else
            {
                giftSlots[giftInventoryIndex].SetSlot(inventoryManager.slots[i].itemData, inventoryManager.slots[i].quantity);
                giftSlots[giftInventoryIndex].SetButton(UIManager.Instance.giftUI); // 선물 UI에 버튼 설정
                giftInventoryIndex++;
            }
        }
    }
}
