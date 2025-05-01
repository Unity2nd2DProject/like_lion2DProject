using System;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
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
    [SerializeField] private GameObject statUIPrefab;

    public GameObject cookingUIPrefab;
    [HideInInspector]
    public CookingUI cookingUI;

    private StatUI statUIInstance;
    
    protected override void Awake()
    {
        base.Awake();
    }

    public void InitializeStatUI(List<Stat> stats)
    {
        Debug.Log("StatUI Initialized");
        Canvas canvas = Canvas.FindAnyObjectByType<Canvas>();
        statUIInstance = Instantiate(statUIPrefab, canvas.transform).GetComponent<StatUI>();
        statUIInstance.Initialize(stats);
        Debug.Log(statUIInstance);
    }

    public void InitializeInventoryAndQuickSlot()
    {
        Canvas canvas = GameObject.FindGameObjectWithTag("MainCanvas").GetComponent<Canvas>(); // 씬에서 Canvas를 찾음
        inventoryUI = canvas.GetComponentInChildren<InventoryUI>(); // 씬에서 InventoryUI를 찾음
        if (inventoryUI == null) // 인벤토리 UI가 null이면 새로 생성
        {
            inventoryUI = Instantiate(inventoryUIPrefab, canvas.transform).GetComponent<InventoryUI>();
            inventoryUI.gameObject.transform.SetAsFirstSibling();
            inventoryUI.GetComponent<InventoryUI>().InitializeInventoryUI();
        }

        quickSlotUI = canvas.GetComponentInChildren<QuickSlotUI>(); // 씬에서 QuickSlotUI를 찾음
        if (quickSlotUI == null) // 퀵슬롯 UI가 null이면 새로 생성
        {
            quickSlotUI = Instantiate(quickSlotUIPrefab, canvas.transform).GetComponent<QuickSlotUI>();
            quickSlotUI.gameObject.transform.SetAsFirstSibling();
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
        Canvas canvas = GameObject.FindGameObjectWithTag("MainCanvas").GetComponent<Canvas>();
        GameObject popup = Instantiate(PopupMessagePrefab, canvas.transform);
        popup.transform.position = position;    
        popup.GetComponent<PopUpMessageUI>().SetMessage(message);
    }

    public void ToggleInventoryAndQuickSlotForTest()
    {

        if(quickSlotUI.gameObject.activeSelf != inventoryUI.gameObject.activeSelf)
        {
            quickSlotUI.gameObject.SetActive(true);
            inventoryUI.gameObject.SetActive(true);
        }
        else
        {
            quickSlotUI.gameObject.SetActive(!quickSlotUI.gameObject.activeSelf);
            inventoryUI.gameObject.SetActive(!inventoryUI.gameObject.activeSelf);
        }
            
    }

    public void ShowQuickSlotUI()
    {
        if (quickSlotUI != null)
        {
            quickSlotUI.gameObject.SetActive(true);
        }
    }



    public void InitializeCookingUI()
    {
        Canvas canvas = Canvas.FindAnyObjectByType<Canvas>();
        cookingUI = canvas.GetComponentInChildren<CookingUI>();
        if (cookingUI == null)
        {
            cookingUI = Instantiate(cookingUIPrefab, canvas.transform).GetComponent<CookingUI>();
            cookingUI.gameObject.SetActive(false);
        }
        
    }

    public void ToggleCookingUI()
    {
        if(cookingUI == null)
        {
            InitializeCookingUI();
        }
        cookingUI.gameObject.SetActive(!cookingUI.gameObject.activeSelf);
        cookingUI.transform.SetAsLastSibling();
    }
}

