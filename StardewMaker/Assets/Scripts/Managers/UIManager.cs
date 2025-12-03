using System;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    [Header("Inventory and QuickSlot")]
    public InventoryUI InventoryUI;
    public QuickSlotUI QuickSlotUI;

    [Header("Popup Message")]
    public GameObject popupMessageUIPrefab;
    [HideInInspector] public PopUpMessageUI PopupUI;
    public GameObject toolTipPrefab;
    private TooltipUI toolTipInstance;

    [Header("Stat UI")]
    [SerializeField] private GameObject statUIPrefab;
    [HideInInspector]
    public StatUI statUIInstance;

    [Header("Cooking UI")]
    public GameObject cookingUIPrefab;
    [HideInInspector]
    public CookingUI cookingUI;

    [Header("Gift UI")]
    public GameObject giftUIPrefab;
    [HideInInspector]
    public GiftUI giftUI;

    [Header("Setting UI")]
    public GameObject soundSettingUIPrefab;
    private SoundSettingUI SoundSettingUIInstance;

    private BaseUI baseUI;

    [Header("Shop UI")]
    public ShopUI ShopUI;

    [Header("Dialogue UI")]
    public GameObject dialogueUI;

    [Header("Quest UI")]
    public GameObject questUI;

    [Header("Fade Image")]
    public GameObject fadeImage;

    // Normal Menu띄우기 액션
    public static event Action<bool> OnNormalMenuRequested;

    [HideInInspector] public Canvas canvas;

    private bool isUIon = false;

    protected override void Awake()
    {
        base.Awake();

        canvas = GameObject.FindGameObjectWithTag("MainCanvas").GetComponent<Canvas>();
    }

    #region 딸 관련 UI 

    public void InitializeStatUI(List<Stat> stats)
    {
        if (statUIInstance == null) // 이미 StatUI가 존재하면 초기화 하지 않음
        {
            statUIInstance = Instantiate(statUIPrefab, canvas.transform).GetComponent<StatUI>();
            statUIInstance.Initialize(stats);
        }
    }


    public void InitializeCookingUI()
    {
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
        cookingUI.cookingInventory.UpdateIngredientInventoryUI();
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
        giftUI = canvas.GetComponentInChildren<GiftUI>();
        if (giftUI == null)
        {
            giftUI = Instantiate(giftUIPrefab, canvas.transform).GetComponent<GiftUI>();
            giftUI.gameObject.SetActive(false);
            giftUI.GetComponent<GiftUI>().GiftInventoryUI.GetComponent<GiftInventoryUI>().UpdateGiftInventory();
            giftUI.GetComponent<GiftUI>().GiftInfoUI.GetComponent<GiftInfoUI>().InitializeGiftInfoUI();
        }
    }

    public void ToggleGiftUI()
    {
        if (giftUI == null)
        {
            InitializeGiftUI();
        }
        giftUI.GetComponent<GiftUI>().GiftInventoryUI.GetComponent<GiftInventoryUI>().UpdateGiftInventory();
        giftUI.GetComponent<GiftUI>().GiftInfoUI.GetComponent<GiftInfoUI>().InitializeGiftInfoUI();
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

    #endregion

    #region 인벤토리 및 퀵슬롯 UI
    public void InitializeInventoryAndQuickSlot()
    {
        InventoryUI.gameObject.transform.SetAsFirstSibling();
        QuickSlotUI.gameObject.transform.SetAsFirstSibling();

        InventoryUI.InitializeInventoryUI();
        QuickSlotUI.InitializeQuickSlotUI();

        if(true) //TODO: 초기화 시점에 따라 변경 필요
        {
            ShowQuickSlotUI();
        }
    }

    public void UpdateInventoryAndQuickSlot()
    {
        QuickSlotUI.UpdateQuickSlotUI();
        InventoryUI.UpdateInventoryUI();
    }

    public void UpdateQuickSlotUI()
    {
        QuickSlotUI.UpdateQuickSlotUI();
    }

    public void UpdateInventoryUI()
    {
        InventoryUI.UpdateInventoryUI();
    }
    public void ShowInventoryUI()
    {
        if (InventoryUI != null)
        {
            InventoryUI.gameObject.SetActive(true);
        }
    }

    public void ShowQuickSlotUI()
    {
        if (QuickSlotUI != null)
        {
            QuickSlotUI.gameObject.SetActive(true);
        }
    }

    #endregion

    public void ShowPopup(string message, Vector3 position = default)
    {
        if (position == default)
        {
            position = Input.mousePosition;
        }

        // 이전 팝업이 있다면 제거
        if (PopupUI != null)
        {
            Destroy(PopupUI);
            PopupUI = null;
        }

        GameObject popup = Instantiate(popupMessageUIPrefab, canvas.transform);
        popup.transform.position = position;
        PopupUI = popup.GetComponent<PopUpMessageUI>();
        PopupUI.SetMessage(message);

    }

    public void HidePopupImmediately()
    {
        if (PopupUI != null)
        {
            Destroy(PopupUI);
            PopupUI = null;
        }
    }

    public void ToggleInventoryByButton()
    {
        UpdateInventoryUI();
        UpdateQuickSlotUI();
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

    public void UpdateMoneyUI()
    {
        if (baseUI == null)
        {
            baseUI = GameObject.FindGameObjectWithTag("MainCanvas").GetComponentInChildren<BaseUI>();
        }
        baseUI.SetMoneyText(InventoryManager.Instance.PlayerMoney.ToString("#,0"));
    }

    public void ToggleSoundSettingUI()
    {
        Debug.Log("ToggleSoundSettingUI");
        if (SoundSettingUIInstance == null)
        {
            SoundSettingUIInstance = Instantiate(soundSettingUIPrefab, canvas.transform).GetComponent<SoundSettingUI>();
            SoundSettingUIInstance.gameObject.SetActive(false);
        }
        SoundSettingUIInstance.gameObject.SetActive(!SoundSettingUIInstance.gameObject.activeSelf);
    }

    public void OpenShopUI()
    {     
        ShopUI.gameObject.SetActive(true);
        InventoryUI.transform.parent = ShopUI.transform;
        ShowInventoryUI();
        InventoryUI.SetShopMode();
        OnUI();
    }

    public void OnUI()
    {
        isUIon = true;
    }

    public void OffUI()
    {
        isUIon = false;
    }

    public bool IsUIOn()
    {
        return isUIon;
    }

    public void ToggleQuestPanel()
    {
        questUI.SetActive(!questUI.activeSelf);
    }
}

