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


    private void Start()
    {
        UpdateInventoryUI();
        cancelButton.onClick.AddListener(OnCancelButtonClicked);
    }

    private void OnEnable()
    {
        UpdateInventoryUI();
    }

    public void UpdateInventoryUI()
    {
        for (int i = 0; i < InventoryManager.Instance.inventorySize; i++)
        {
            inventorySlotUIs[i].UpdateSlot(InventoryManager.Instance.slots[i]);
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
        gameObject.SetActive(!gameObject.activeSelf);
    }

    public void ShowInventory()
    {
        gameObject.SetActive(true);
    }

    public void HideInventory()
    {
        gameObject.SetActive(false);
    }

    public ItemData GetSelectedItem()
    {
        return null;
    }
}
