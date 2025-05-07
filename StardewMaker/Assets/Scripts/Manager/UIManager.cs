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
    public GameObject popupMessagePrefab;
    public GameObject toolTipPrefab;
    private TooltipUI toolTipInstance;

    [Header("Stat UI")]
    [SerializeField] private GameObject statUIPrefab;
    private StatUI statUIInstance;


    [Header("Cooking UI")]
    public GameObject cookingUIPrefab;
    [HideInInspector]
    public CookingUI cookingUI;

    [Header("Gift UI")]
    public GameObject giftUIPrefab;
    [HideInInspector]
    public GiftUI giftUI;

    // Normal Menu띄우기 액션
    public static event Action<bool> OnNormalMenuRequested;


    protected override void Awake()
    {
        base.Awake();
    }

    public void InitializeStatUI(List<Stat> stats)
    {
        Canvas canvas = GameObject.FindGameObjectWithTag("MainCanvas").GetComponent<Canvas>();
        statUIInstance = Instantiate(statUIPrefab, canvas.transform).GetComponent<StatUI>();
        statUIInstance.Initialize(stats);
        Debug.Log(statUIInstance);
    }

    public void InitializeInventoryAndQuickSlot()
    {
        Canvas canvas = GameObject.FindGameObjectWithTag("MainCanvas").GetComponent<Canvas>(); // 씬에서 Canvas를 찾음
        inventoryUI = canvas.GetComponentInChildren<InventoryUI>();
        if (inventoryUI == null) // 인벤토리 UI가 null이면 새로 생성
        {
            inventoryUI = Instantiate(inventoryUIPrefab, canvas.transform).GetComponent<InventoryUI>();
            inventoryUI.gameObject.transform.SetAsFirstSibling();
            inventoryUI.InitializeInventoryUI();
        }

        quickSlotUI = canvas.GetComponentInChildren<QuickSlotUI>();
        if (quickSlotUI == null) // 퀵슬롯 UI가 null이면 새로 생성
        {
            quickSlotUI = Instantiate(quickSlotUIPrefab, canvas.transform).GetComponent<QuickSlotUI>();
            quickSlotUI.gameObject.transform.SetAsFirstSibling();
            quickSlotUI.InitializeQuickSlotUI();
        }
    }

    public void UpdateInventoryAndQuickSlot()
    {
        quickSlotUI.UpdateQuickSlotUI();
        inventoryUI.UpdateInventoryUI();
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
        if (position == default)
        {
            position = Input.mousePosition;
        }
        Canvas canvas = GameObject.FindGameObjectWithTag("MainCanvas").GetComponent<Canvas>();
        GameObject popup = Instantiate(popupMessagePrefab, canvas.transform);
        popup.transform.position = position;
        popup.GetComponent<PopUpMessageUI>().SetMessage(message);
    }

    public void ToggleInventoryAndQuickSlotForTest()
    {

        if (quickSlotUI.gameObject.activeSelf != inventoryUI.gameObject.activeSelf)
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
        Canvas canvas = GameObject.FindGameObjectWithTag("MainCanvas").GetComponent<Canvas>();
        cookingUI = canvas.GetComponentInChildren<CookingUI>();
        if (cookingUI == null)
        {
            cookingUI = Instantiate(cookingUIPrefab, canvas.transform).GetComponent<CookingUI>();
            cookingUI.gameObject.SetActive(false);
        }

    }

    public void ToggleCookingUI()
    {
        if (cookingUI == null)
        {
            InitializeCookingUI();
        }
        cookingUI.gameObject.SetActive(!cookingUI.gameObject.activeSelf);
        cookingUI.transform.SetAsLastSibling();
    }

    public void CloseCookingUI()
    {
        cookingUI.gameObject.SetActive(false);

        if (!cookingUI.gameObject.activeSelf) // 열려있다면 닫고 메인메뉴 띄우기
        {
            OnNormalMenuRequested?.Invoke(true);
        }
    }

    public void InitializeGiftUI()
    {
        Canvas canvas = GameObject.FindGameObjectWithTag("MainCanvas").GetComponent<Canvas>();
        giftUI = canvas.GetComponentInChildren<GiftUI>();
        if (giftUI == null)
        {
            giftUI = Instantiate(giftUIPrefab, canvas.transform).GetComponent<GiftUI>();
            giftUI.gameObject.SetActive(false);
            giftUI.GetComponent<GiftUI>().GiftInventoryUI.GetComponent<GiftInventoryUI>().InitializeGiftInventoryUI(); // 선물 인벤토리 UI 초기화
            giftUI.GetComponent<GiftUI>().GiftInventoryUI.GetComponent<GiftInventoryUI>().UpdateGiftInventory();
        }
    }

    public void ToggleGiftUI()
    {
        if (giftUI == null)
        {
            InitializeGiftUI();
        }
        giftUI.gameObject.SetActive(!giftUI.gameObject.activeSelf);
        giftUI.transform.SetAsLastSibling();
    }

    public void CloseGiftUI()
    {
        giftUI.gameObject.SetActive(false);

        if (!giftUI.gameObject.activeSelf) // 열려있다면 닫고 메인메뉴 띄우기
        {
            OnNormalMenuRequested?.Invoke(true);
        }
    }

    public void ShowTooltip(ItemData itemdata, Vector3 position)
    {
        if (toolTipInstance == null)
        {
            toolTipInstance = Instantiate(toolTipPrefab, GameObject.FindGameObjectWithTag("MainCanvas").transform).GetComponent<TooltipUI>();
        }
        toolTipInstance.ShowTooltip(itemdata, position);
        toolTipInstance.gameObject.SetActive(true);
    }
    public void HideTooltip()
    {
        if (toolTipInstance != null)
        {
            toolTipInstance.HideTooltip();
            toolTipInstance.gameObject.SetActive(false);
        }
    }


}

