using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class QuickSlotUI : MonoBehaviour
{
    InventoryManager inventoryManager;
    public List<ItemSlotUI> quickSlotSlotUIs = new List<ItemSlotUI>();
    public GameObject currentSelectedCursor;

    private void ShowQuickSlot()
    {
        gameObject.SetActive(true);
    }

    private void HideQuickSlot()
    {
        gameObject.SetActive(false);
    }

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
        if (currentSelectedCursor != null && quickSlotSlotUIs != null)
        {
            currentSelectedCursor.transform.SetParent(quickSlotSlotUIs[inventoryManager.currentSelectedQuickSlotIndex].transform);
            currentSelectedCursor.transform.localPosition = new Vector3(0, 0, 0);
        }

    }
}
