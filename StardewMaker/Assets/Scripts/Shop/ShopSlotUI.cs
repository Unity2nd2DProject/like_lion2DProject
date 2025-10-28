using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class ShopSlotUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public static bool IsDragging { get; private set; }

    [Header("UI Components")]
    public Image itemIcon;
    public TextMeshProUGUI itemNameText;
    public TextMeshProUGUI priceText;
    public Button buyButton;

    private ItemData itemData;
    private ShopUIController shopUI;

    // 슬롯 초기화
    public void Setup(ItemData data, ShopUIController shopUI)
    {
        itemData = data;
        this.shopUI = shopUI;

        // UI 갱신
        itemIcon.sprite = itemData.icon;
        itemNameText.text = itemData.itemName;
        priceText.text = itemData.buyPrice.ToString("#,0");

        // 버튼 리스너 재설정
        buyButton.onClick.RemoveAllListeners();
        buyButton.onClick.AddListener(OnBuyButtonClicked);
    }

    // 구매 버튼 클릭 시 실행
    private void OnBuyButtonClicked()
    {
        // 구매 시도
        bool success = ShopManager.Instance.Buy(itemData);

        // 성공하면 UI 갱신
        if (success)
        {
            UIManager.Instance.UpdateInventoryUI();
        }
        else
        {
            // 구매 실패 처리 (예: 알림 팝업)
            Debug.Log("구매 실패: 금액이 부족하거나 인벤토리에 공간이 없습니다.");
        }
    }

    // 마우스가 슬롯 위로 올라왔을 때
    public void OnPointerEnter(PointerEventData eventData)
    {
        // 버튼 위가 아니면 팝업 표시
        if (!IsPointerOver(buyButton.gameObject))
        {
            shopUI.ShowItemInfoUI(itemData, eventData.position);
        }
    }

    // 마우스가 슬롯에서 벗어났을 때
    public void OnPointerExit(PointerEventData eventData)
    {
        shopUI.HideItemInfoUI();
    }

    // 실제 마우스가 특정 오브젝트 위에 있는지 확인
    private bool IsPointerOver(GameObject target)
    {
        PointerEventData pointerData = new PointerEventData(EventSystem.current)
        {
            position = Input.mousePosition
        };

        var results = new System.Collections.Generic.List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerData, results);

        foreach (var result in results)
        {
            if (result.gameObject == target || result.gameObject.transform.IsChildOf(target.transform))
            {
                return true;
            }
        }

        return false;
    }
}