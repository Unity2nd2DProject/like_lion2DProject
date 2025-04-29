using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SellPopupUI : MonoBehaviour
{
    [Header("UI References")]
    public TMP_InputField quantityInput;
    public TextMeshProUGUI totalPriceText;
    public Button confirmButton;
    public Button cancelButton;
    public Button increaseButton;
    public Button decreaseButton;

    private ShopManager shopManager;
    private ShopUI shopUI;

    private ItemSlot currentSlot;
    private int unitPrice;
    private int maxQty;
    private int currentQty;

    // 초기화 (상점 매니저 및 UI 참조)
    public void Init(ShopManager manager, ShopUI ui)
    {
        shopManager = manager;
        shopUI = ui;
    }

    // 팝업 표시 및 데이터 설정
    public void Show(ItemSlot slot)
    {
        currentSlot = slot;
        unitPrice = slot.itemData.sellPrice;
        maxQty = slot.quantity;

        currentQty = 1; // 항상 기본 수량으로 초기화

        UpdateUI();

        // 리스너 초기화 및 연결
        AddInputListener(quantityInput, OnQuantityChanged);
        AddButtonListener(increaseButton, OnIncrease);
        AddButtonListener(decreaseButton, OnDecrease);
        AddButtonListener(confirmButton, OnConfirm);
        AddButtonListener(cancelButton, Hide);

        // 판매 팝업 표시 시 상호작용 잠금
        shopUI?.LockInteraction(); // 상점 UI 잠금

        gameObject.SetActive(true);
    }
    
    // 버튼 클릭 리스너 연결 함수
    private void AddButtonListener(Button button, UnityEngine.Events.UnityAction action)
    {
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(action);
    }

    // 입력값 변경 리스너 연결 함수
    private void AddInputListener(TMP_InputField inputField, UnityEngine.Events.UnityAction<string> action)
    {
        inputField.onValueChanged.RemoveAllListeners();
        inputField.onValueChanged.AddListener(action);
    }

    // 판매 수량 및 총액 UI 갱신
    void UpdateUI()
    {
        quantityInput.text = currentQty.ToString();
        totalPriceText.text = (unitPrice * currentQty).ToString("#,0");
    }

    // 수량 input 값 변경
    private void OnQuantityChanged(string text)
    {
        if (int.TryParse(text, out int val))
        {
            currentQty = Mathf.Clamp(val, 1, maxQty);
            UpdateUI();
        }
    }

    //판매 수량 늘림 버튼
    private void OnIncrease()
    {
        if (currentQty < maxQty)
        {
            currentQty++;
            UpdateUI();
        }
    }

    // 판매 수량 줄임 버튼
    private void OnDecrease()
    {
        if (currentQty > 1)
        {
            currentQty--;
            UpdateUI();
        }
    }

    // 판매 확정 처리
    void OnConfirm()
    {
        if (shopUI == null)
        {
            return;
        }

        bool success = ShopManager.Instance.Sell(currentSlot.itemData, currentQty);

        if (success)
        {
            shopUI.UpdateUI(); // 상점 슬롯 갱신
            UIManager.Instance.UpdateInventoryUI(); // 인벤토리 갱신
            Hide(); // 팝업 닫기
        }
    }

    // 팝업 숨기기 및 상호작용 복원
    public void Hide()
    {
        gameObject.SetActive(false);
        shopUI?.UnlockInteraction();
    }
}
