using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class UIManager : Singleton<UIManager>
{
    [Header("Inventory and QuickSlot")]
    public GameObject inventoryUIPrefab;
    public GameObject quickSlotUIPrefab;

    [HideInInspector]
    public InventoryUI inventoryUI;
    private QuickSlotUI quickSlotUI;


    [Header("Popup Message")]
    public GameObject PopupMessagePrefab;

    [Header("Stat UI")]
    [SerializeField] private StatUI statUIPrefab;
    [SerializeField] private Transform statUIParent;

    private StatUI statUIInstance;
    private Canvas canvas;

    
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
        Debug.Log($"[{nameof(UIManager)}] InitializeInventoryAndQuickSlot()");
        Debug.Log(canvas);
        if (inventoryUI == null) // 인벤토리 UI가 null이면 새로 생성
        {
            Debug.Log(inventoryUIPrefab);
            inventoryUI = Instantiate(inventoryUIPrefab, canvas.transform).GetComponent<InventoryUI>();
            inventoryUI.GetComponent<InventoryUI>().InitializeInventoryUI();
            Debug.Log(inventoryUI);
        }

        if (quickSlotUI == null) // 퀵슬롯 UI가 null이면 새로 생성
        {
            quickSlotUI = Instantiate(quickSlotUIPrefab, canvas.transform).GetComponent<QuickSlotUI>();
            quickSlotUI.InitializeQuickSlotUI();
        }
    }

    public void UpdateQuickSlotUI()
    {
        quickSlotUI.UpdateQuickSlotUI();
    }

    public void UpdateInventoryUI()
    {
        inventoryUI.UpdateInventoryUI();
    }

    public void ShowPopup(string message, Vector3 position = default)
    {
        if(position == default)
        {
            position = Input.mousePosition;
        }

        GameObject popup = Instantiate(PopupMessagePrefab, canvas.transform);
        popup.transform.position = position;    
        popup.GetComponent<PopUpMessageUI>().SetMessage(message);
    }
}

