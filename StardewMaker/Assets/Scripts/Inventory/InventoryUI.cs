using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using System;
using System.Collections.Generic;

public class InventoryUI : MonoBehaviour
{
    public List<SlotUI> inventorySlotUIs = new List<SlotUI>();
    public Button cancelButton;

    private InventoryManager inventoryManager;

    private void Awake()
    {
        HideInventory();
    }

    public void InitializeInventoryUI()
    {
        inventoryManager = InventoryManager.Instance;
        cancelButton.onClick.AddListener(OnCancelButtonClicked);
    }

    public void UpdateInventoryUI()
    {
        for (int i = 0; i < inventoryManager.inventorySize; i++)
        {
            inventorySlotUIs[i].UpdateSlot(inventoryManager.slots[i]);
        }
    }

    private void OnCancelButtonClicked()
    {
        ToggleInventory();

        if (ShopUI.Instance != null && ShopUI.Instance.gameObject.activeSelf)
        {
            ShopUI.Instance.Close();
        }
    }

    public void ToggleInventory()
    {
        UpdateInventoryUI();
        gameObject.SetActive(!gameObject.activeSelf);
        UIManager.Instance.HideTooltip();
    }

    public void ShowInventory()
    {
        gameObject.SetActive(true);
    }

    public void HideInventory()
    {
        gameObject.SetActive(false);
        UIManager.Instance.HideTooltip();
    }

    public ItemData GetSelectedItem()
    {
        return null;
    }
}
