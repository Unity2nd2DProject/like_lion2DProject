using System;
using System.Collections.Generic;
using UnityEngine;

public class QuickSlotUI : MonoBehaviour
{
    InventoryManager inventoryManager;
    public List<SlotUI> quickSlotSlotUIs = new List<SlotUI>();
    public GameObject currentSelectedCursor;

    private void Init(InventoryManager inventoryManager)
    {
        this.inventoryManager = inventoryManager;
        UpdateUI();
    }

    public void UpdateUI()
    {
        for (int i = 0; i < inventoryManager.quickSlotSize; i++)
        {
            quickSlotSlotUIs[i].UpdateSlot(inventoryManager.slots[i]);
        }
    }

    internal void UpdateSelectedSlot()
    {
        currentSelectedCursor.transform.SetParent(quickSlotSlotUIs[inventoryManager.currentSelectedIndex].transform);
        currentSelectedCursor.transform.localPosition = new Vector3(-50, 50); // 위치 조정
    }
}
