using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using System;
using System.Collections.Generic;

public class InventoryUI : Singleton<InventoryUI>
{

    public Inventory inventory;

    public GameObject inventorGrid; // 필요한가?

    public List<InventorySlotUI> inventorySlotUIs = new List<InventorySlotUI>();

    public Button cancelButton;

    protected override void Awake()
    {
        base.Awake();

        this.gameObject.SetActive(false);
        cancelButton.onClick.AddListener(OnCancelButtonClicked);
    }

    private void OnEnable()
    {
        UpdateInventoryUI();
    }

    public void UpdateInventoryUI()
    {
        for (int i = 0; i < inventory.slots.Count; i++)
        {
            inventorySlotUIs[i].UpdateSlot(inventory.slots[i]);
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
