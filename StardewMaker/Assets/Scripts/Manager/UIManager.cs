using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class UIManager : Singleton<UIManager>
{
    [Header("Inventory and QuickSlot")]
    public GameObject InventoryUI;
    public GameObject QuickSlotUI;

    [Header("Popup Message")]
    public GameObject PopupMessagePrefab;

    [Header("Stat UI")]
    [SerializeField] private StatUI statUIPrefab;
    [SerializeField] private Transform statUIParent;

    private StatUI statUIInstance;

    public Canvas cnavas;

    
    protected override void Awake()
    {
        base.Awake();
    }

    public void InitializeStatUI(List<Stat> stats)
    {
        statUIInstance = Instantiate(statUIPrefab, statUIParent);
        statUIInstance.Initialize(stats);
    }

    public void InitializeInventoryAndQuickSlot()
    {
        InventoryUI.GetComponent<InventoryUI>().InitializeInventoryUI();
        QuickSlotUI.GetComponent<QuickSlotUI>().InitializeQuickSlotUI();
    }

    public void UpdateInventoryAndQuickSlot()
    {
        QuickSlotUI.GetComponent<QuickSlotUI>().UpdateQuickSlotUI();
        InventoryUI.GetComponent<InventoryUI>().UpdateInventoryUI();
    }

    public void UpdateQuickSlotUI()
    {
        QuickSlotUI.GetComponent<QuickSlotUI>().UpdateQuickSlotUI();
    }

    public void UpdateInventoryUI()
    {
        InventoryUI.GetComponent<InventoryUI>().UpdateInventoryUI();
    }

    public void ShowPopup(string message, Vector3 position = default)
    {
        if(position == default)
        {
            position = Input.mousePosition;
        }

        GameObject popup = Instantiate(PopupMessagePrefab, cnavas.transform);
        popup.transform.position = position;    
        popup.GetComponent<PopUpMessageUI>().SetMessage(message);
    }
}

