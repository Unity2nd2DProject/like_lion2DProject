using System;
using UnityEngine;

public class CookingInventory : MonoBehaviour
{
    Inventory inventory;

    public SlotedItemUI[] slotedItemUIs; // 슬롯 아이템 UI 배열

    private void Awake()
    {
        inventory = Inventory.Instance; // Inventory 인스턴스 가져오기
    }
    private void Start()
    {
        UpdateUI(); // UI 업데이트
    }

    private void UpdateUI()
    {
        int cookInventoryIDX = 0;
        for (int i = 0; i < inventory.inventorySize; i++)
        {
            // 슬롯이 비어있지 않고, 아이템 타입이 재료인 경우
            if (!inventory.slots[i].IsEmpty() && inventory.slots[i].itemData.itemType == ItemType.Ingredient)          {

                slotedItemUIs[cookInventoryIDX].SetSlot(inventory.slots[i].itemData, inventory.slots[i].quantity);
                cookInventoryIDX++;
            }
        }
    }
}
