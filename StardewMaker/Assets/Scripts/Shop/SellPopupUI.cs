using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SellPopupUI : MonoBehaviour
{
    [Header("UI References")]
    public TMP_InputField quantityInput;
    public TextMeshProUGUI totalPriceText;

    public Button increaseButton;
    public Button decreaseButton;
    public Button confirmButton;
    public Button cancleButton;

    private ItemSlot currentSlot;
    private int unitPrice;
    private int maxQty;
    private int currentQty;

    public void SetItemSlot(ItemSlot slot)
    {
        currentSlot = slot;
        unitPrice = slot.itemData.sellPrice;
        maxQty = slot.quantity;

        currentQty = 1;

        UpdateUI();
        quantityInput.onValueChanged.AddListener(OnQuantityChanged);
        increaseButton.onClick.AddListener(OnIncrease);
        decreaseButton.onClick.AddListener(OnDecrease);
        confirmButton.onClick.AddListener(OnConfirm);
        cancleButton.onClick.AddListener(Close);
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
        bool success = ShopManager.Instance.Sell(currentSlot.itemData, currentQty);

        if (success)
        {
            UIManager.Instance.UpdateInventoryUI(); // 인벤토리 갱신
            Close(); // 팝업 닫기
        }
    }

    // 팝업 숨기기 및 상호작용 복원
    public void Close()
    {
        Destroy(gameObject);
    }
}
