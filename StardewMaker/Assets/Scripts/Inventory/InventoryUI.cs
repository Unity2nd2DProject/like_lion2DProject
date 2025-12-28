using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using System;
using System.Collections.Generic;

public class InventoryUI : MonoBehaviour
{
    public List<ItemSlotUI> inventorySlotUIs = new List<ItemSlotUI>();
    public Button cancelButton;

    private InventoryManager inventoryManager;

    private void Awake()
    {
        HideInventory();
    }

    public void InitializeInventoryUI()
    {
        inventoryManager = InventoryManager.Instance;
        cancelButton.onClick.RemoveAllListeners();
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
        if (GameManager.Instance.currentMode == GameMode.HOME)
        {
            UIManager.Instance.ToggleInventoryByButton();
        }
        else
        {
            HideInventory();
        }
    }

    public void ToggleInventory()
    {
        UpdateInventoryUI();
        gameObject.SetActive(!gameObject.activeSelf);
        UIManager.Instance.HideTooltip();
    }

    public void HideInventory()
    {
        gameObject.SetActive(false);
        UIManager.Instance.HideTooltip();
    }

    public void SetShopMode()
    {
        cancelButton.onClick.RemoveAllListeners();
        cancelButton.onClick.AddListener(OnCancelButtonClickedShop);
    }
    
    public void OnCancelButtonClickedShop()
    {
        Debug.Log("상점 닫기");
        UIManager.Instance.ShopUI.CloseShopUI();
        UIManager.Instance.InventoryUI.HideInventory();
        InitializeInventoryUI();

        UIManager.Instance.dialogueUI.SetActive(true);
        UIManager.Instance.dialogueUI.GetComponent<DialogueController>().EndBuissness();
        this.gameObject.transform.parent = UIManager.Instance.canvas.transform;        
        
        UIManager.Instance.OffUI();
    }
}
