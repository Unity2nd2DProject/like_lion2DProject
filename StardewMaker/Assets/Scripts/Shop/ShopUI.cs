using UnityEngine;
using UnityEngine.UI;

public class ShopUI : MonoBehaviour
{
    public static ShopUI Instance { get; private set; }

    [Header("UI References")]
    public GameObject shopRoot; // 상점 UI 전체
    public CanvasGroup canvasGroup; // UI 상호작용 제어용
    public Transform buySlotParent; // 상점 구매 슬롯 부모
    public GameObject slotPrefab; // 슬롯 프리팹
    public RectTransform scrollRectTransform;

    [Header("Manager References")]
    public ShopManager shopManager;
    public SellPopupUI sellPopup;
    public ItemInfoPopupUI itemInfoPopup;

    [Header("Inventory Reference")]
    // [SerializeField] private Transform inventoryParent;
    private SlotUI[] inventorySlots;
    
    [Header("Scroll")]
    public RectTransform scrollViewport;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        // 인벤토리 슬롯 자동 수집
        inventorySlots = UIManager.Instance.inventoryUI.GetComponentsInChildren<SlotUI>(true);
    }

    void OnEnable()
    {
        if (sellPopup != null)
        {
            sellPopup.Init(shopManager, this);
        }

        EnableInventorySellMode(true);
        UpdateUI();
    }

    void OnDisable()
    {
        EnableInventorySellMode(false);
    }

    // 인벤토리 슬롯들에 판매 모드 활성/비활성 설정
    public void EnableInventorySellMode(bool enable)
    {
        foreach (var slot in inventorySlots)
        {
            slot.SetSellMode(enable, enable ? sellPopup : null);
        }
    }

    // 상점 슬롯 UI 새로고침
    public void UpdateUI()
    {
        if (shopManager == null || shopManager.shopItems == null)
        {
            return;
        }

        foreach (Transform child in buySlotParent)
            Destroy(child.gameObject);

        foreach (var item in shopManager.shopItems)
        {
            var go = Instantiate(slotPrefab, buySlotParent);
            var ui = go.GetComponent<ShopSlotUI>();
            if (ui == null)
            {
                continue;
            }

            ui.Setup(item, shopManager, itemInfoPopup, this);
        }
    }

    public void Close()
    {
        // 판매 팝업 닫기
        if (sellPopup != null)
        {
            sellPopup.Hide();
        }

        // 상점 UI 전체 비활성화
        if (shopRoot != null)
        {
            shopRoot.SetActive(false);
        }

        // 시간 정지 해제 (필요한 경우)
        Time.timeScale = 1f;
    }

    // 상점 UI 상호작용 잠금 (판매 팝업이 켜졌을 때 호출)
    public void LockInteraction()
    {
        if (canvasGroup != null)
        {
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
        }
    }

    // 상점 UI 상호작용 해제 (팝업 닫을 때 호출)
    public void UnlockInteraction()
    {
        if (canvasGroup != null)
        {
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;
        }
    }
}