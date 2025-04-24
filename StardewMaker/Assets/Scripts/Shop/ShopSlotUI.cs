using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class ShopSlotUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("UI Components")]
    public Image itemIcon;
    public TextMeshProUGUI itemNameText;
    public TextMeshProUGUI priceText;
    public Button buyButton;

    [Header("Popup Settings")]
    [SerializeField] private float safeZonePadding = 40f;

    private ItemData itemData;
    private ShopManager shopManager;
    private ItemInfoPopupUI infoPopup;
    private ShopUI shopUI;

    private bool isPointerOverSlot = false;

    // 슬롯 초기화
    public void Setup(ItemData data, ShopManager manager, ItemInfoPopupUI popup, ShopUI ui)
    {
        itemData = data;
        shopManager = manager;
        infoPopup = popup;
        shopUI = ui;

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
        bool success = shopManager.Buy(itemData, 1);

        // 성공하면 UI 갱신
        if (success)
        {
            shopUI.UpdateUI();  // 상점 슬롯 갱신
            InventoryUI.Instance.UpdateInventoryUI(); // 인벤토리 갱신
        }
    }

    // 마우스가 슬롯 위로 올라왔을 때
    public void OnPointerEnter(PointerEventData eventData)
    {
        isPointerOverSlot = true;

        // 드래그 중이면 팝업을 띄우지 않음
        if (SlotedItemUI.IsDragging)
        {
            return;
        }

        // 버튼 위가 아니면 팝업 표시
        if (!IsPointerOver(buyButton.gameObject))
        {
            infoPopup.Show(itemData, eventData.position);
        }
    }

    // 마우스가 슬롯에서 벗어났을 때
    public void OnPointerExit(PointerEventData eventData)
    {
        isPointerOverSlot = false;
        infoPopup.Hide();
    }

    void Update()
    {
        // 마우스가 슬롯에 올라가 있고, 아이템 데이터나 팝업이 없거나, 드래그 중일 경우 실행되지 않음
        if (!isPointerOverSlot || itemData == null || infoPopup == null || SlotedItemUI.IsDragging)
            return;

        Vector2 mousePos = Input.mousePosition;

        if (IsPointerNearButton(mousePos))
        {
            if (infoPopup.gameObject.activeSelf)
            {
                infoPopup.Hide();
            }
        }
        else
        {
            if (!infoPopup.gameObject.activeSelf)
            {
                infoPopup.Show(itemData, mousePos);
            }

            // 마우스 움직일 때 팝업 위치도 부드럽게 업데이트
            infoPopup.Follow(mousePos);
        }
    }

    private bool IsPointerNearButton(Vector2 pointerPosition)
    {
        if (buyButton == null)
        {
            return false;
        }

        RectTransform rect = buyButton.GetComponent<RectTransform>();

        // 버튼의 확장된 영역 계산
        Vector3[] corners = new Vector3[4];
        rect.GetWorldCorners(corners);

        // 버튼 주변으로 패딩 영역 확장
        Rect extendedRect = new Rect(
            corners[0].x - safeZonePadding,
            corners[0].y - safeZonePadding,
            (corners[2].x - corners[0].x) + safeZonePadding * 2,
            (corners[2].y - corners[0].y) + safeZonePadding * 2
        );

        bool isInside = extendedRect.Contains(pointerPosition);

        return isInside;
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