using System;
using System.Collections.Generic;
using UnityEngine;

public class QuickSlotUI : MonoBehaviour
{
    InventoryManager inventoryManager;
    public List<SlotUI> quickSlotSlotUIs = new List<SlotUI>();
    public GameObject currentSelectedCursor;

    public void InitializeQuickSlotUI()
    {
        inventoryManager = InventoryManager.Instance;
        UpdateQuickSlotUI();
    }

    public void UpdateQuickSlotUI()
    {
        for (int i = 0; i < inventoryManager.quickSlotSize; i++)
        {
            quickSlotSlotUIs[i].UpdateSlot(inventoryManager.slots[inventoryManager.inventorySize + i]);
        }
        UpdateSelectedSlot();
    }

    private void UpdateSelectedSlot()
    {
        currentSelectedCursor.transform.SetParent(quickSlotSlotUIs[inventoryManager.currentSelectedQuickSlotIndex].transform);
        currentSelectedCursor.transform.localPosition = new Vector3(0, 0, 0);
        Debug.Log(currentSelectedCursor.transform.localPosition);
    }
}
