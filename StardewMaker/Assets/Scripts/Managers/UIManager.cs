using System;
using System.Collections.Generic;
using UnityEditorInternal.Profiling.Memory.Experimental;
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

    [Header("Setting UI")]
    public GameObject optionUI;
    public GameObject soundSettingUIPrefab;
    private SoundSettingUI SoundSettingUIInstance;

    [Header("Item Add Effect")]
    public GameObject itemAddEffectPrefab;


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

    private void Update()
    {
        if(!isUIon)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                ToggleInventoryUI();
            }

            if (Input.GetKeyDown(KeyCode.Q))
            {
                ToggleQuestPanel();
            }
        }
    }

    #region 인벤토리 UI    
   
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
    public void ToggleInventoryUI()
    {
        UpdateInventoryUI();
        UpdateQuickSlotUI();
        InventoryUI.gameObject.SetActive(!InventoryUI.gameObject.activeSelf);
    }

    #endregion

    #region 퀵슬롯 UI
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

    public void PlayItemAddEffect(ItemData itemData)
    {
        Canvas canvas = GameObject.FindGameObjectWithTag("MainCanvas").GetComponent<Canvas>();
        if (itemAddEffectPrefab == null || canvas.transform == null)
        {
            return;
        }

        Vector3 screenPos = new Vector3(Screen.width / 2 + 100f, Screen.height / 2 + 100f, 0f);

        GameObject effectObj = Instantiate(itemAddEffectPrefab, canvas.transform);
        effectObj.transform.SetAsLastSibling(); 
        effectObj.GetComponent<ItemAddEffect>().Play(itemData, screenPos);
    }

    public void CloasAllUI()
    {
        InventoryUI.HideInventory();
        QuickSlotUI.gameObject.SetActive(false);
    }

    public void ShowDialogueUI()
    {
        CloasAllUI();
        dialogueUI.SetActive(true);
    }
}

