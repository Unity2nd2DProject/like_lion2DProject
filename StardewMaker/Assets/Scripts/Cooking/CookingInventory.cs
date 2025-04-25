using System;
using UnityEngine;

public class CookingInventory : MonoBehaviour
{
    Inventory inventory;
    QuickSlotManager quickSlotManager;

    public IngredientSlotUI[] ingredientSlots; // 슬롯 아이템 UI 배열

    private void Awake()
    {
        inventory = Inventory.Instance; // Inventory 인스턴스 가져오기
        quickSlotManager = QuickSlotManager.Instance; // QuickSlotManager 인스턴스 가져오기
    }
    private void Start()
    {
        UpdateUI(); // UI 업데이트
    }

    public void UpdateUI()
    {
        int cookInventoryIndex = 0;

        for (int i = 0; i < inventory.inventorySize; i++)
        {
            // 슬롯이 비어있지 않고, 아이템 타입이 재료인 경우
            if (!inventory.slots[i].IsEmpty() && inventory.slots[i].itemData.itemType == ItemType.Ingredient)          {

                ingredientSlots[cookInventoryIndex].SetSlot(inventory.slots[i].itemData, inventory.slots[i].quantity);
                cookInventoryIndex++;
            }
        }
        for(int i = 0; i < quickSlotManager.slots.Count; i++)
        {
            if (!quickSlotManager.slots[i].IsEmpty() && quickSlotManager.slots[i].itemData.itemType == ItemType.Ingredient)
            {
                ingredientSlots[cookInventoryIndex].SetSlot(quickSlotManager.slots[i].itemData, quickSlotManager.slots[i].quantity);
                cookInventoryIndex++;
            }
        }
    }
}
