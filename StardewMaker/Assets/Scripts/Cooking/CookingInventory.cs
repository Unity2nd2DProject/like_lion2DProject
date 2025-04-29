using System;
using UnityEngine;

public class CookingInventory : MonoBehaviour
{
    InventoryManager inventoryManager;

    public IngredientSlotUI[] ingredientSlots; // 슬롯 아이템 UI 배열

    private void Awake()
    {
        inventoryManager = InventoryManager.Instance;
    }
    private void Start()
    {
        UpdateIngredientInventoryUI(); // 재료 UI 업데이트
    }

    public void UpdateIngredientInventoryUI()
    {
        int cookInventoryIndex = 0;

        for (int i = 0; i < inventoryManager.inventorySize + inventoryManager.quickSlotSize; i++)
        {
            if (!inventoryManager.slots[i].IsEmpty() && inventoryManager.slots[i].itemData.itemType == ItemType.Ingredient)          {

                ingredientSlots[cookInventoryIndex].SetSlot(inventoryManager.slots[i].itemData, inventoryManager.slots[i].quantity);
                cookInventoryIndex++;
            }
        }
    }
}
