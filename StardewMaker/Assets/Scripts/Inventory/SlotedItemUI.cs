using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using System;
using UnityEditor.ShaderGraph.Internal;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;


public class SlotedItemUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler
{
    public static bool IsDragging { get; private set; }

    private ItemData itemData;
    public Image icon;
    public TextMeshProUGUI quantityText;
    public Button slotButton;
    public GameObject quantityTextBox;

    private Transform originalParent;

    private Canvas canvas;
    private CanvasGroup canvasGroup;

    public ItemSlot itemSlot;

    private void Awake()
    {
        canvas = GetComponentInParent<Canvas>();
        canvasGroup = GetComponent<CanvasGroup>();
        slotButton.onClick.AddListener(OnSlotClicked);
    }

    public void SetSlot(ItemData _item, int quantity, ItemSlot realSlot)
    {
        itemData = _item;
        itemSlot = realSlot;

        if (_item != null)
        {
            icon.enabled = true;
            icon.sprite = _item.icon;
            quantityText.text = _item.isStackable ? quantity.ToString() : "";

            if (_item.isStackable)
            {
                quantityTextBox.SetActive(true);
            }
        }
        else
        {
            icon.enabled = false;
            quantityText.text = "";
            quantityTextBox.SetActive(false);
        }
    }

    public ItemData GetItemData()
    {
        return itemData;
    }

    private void OnSlotClicked()
    {
        if (itemData != null)
        {
            // TODO: 아이템 클릭 시 행동 정의
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (itemData != null)
        {
            UIManager.Instance.ShowTooltip(itemData, transform.position);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        UIManager.Instance.HideTooltip();
    }

    #region Drag & Drop
    public void OnBeginDrag(PointerEventData eventData)
    {
        IsDragging = true;  // 드래그 시작

        originalParent = transform.parent; // 드래그 시작 슬롯 저장

        if (itemData == null)
        {
            return;
        }

        // 드래그 중에 아이템 정보 팝업 숨기기
        if (ItemInfoPopupUI.Instance != null)
        {
            ItemInfoPopupUI.Instance.Hide();
        }

        transform.SetParent(canvas.transform);
        transform.SetAsLastSibling(); // 드래그한 아이템을 가장 위로 올림
        canvasGroup.blocksRaycasts = false; // 드래그 중에는 다른 UI 요소와의 상호작용을 비활성화
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (itemData == null)
        {
            return;
        }
        transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        IsDragging = false;
        canvasGroup.blocksRaycasts = true; // 드래그 종료 후 다른 UI 요소와의 상호작용 활성화
        transform.SetParent(originalParent); // 원래 부모로 돌아감

        if (itemData == null)
        {
            ReturnToOriginalPosition();
            return;
        }
        GameObject dropTarget = eventData.pointerEnter;

        if (dropTarget == null)
        {
            ReturnToOriginalPosition();
            return;
        }

        SlotUI targetSlot = dropTarget.GetComponentInParent<SlotUI>();
        if (targetSlot != null)
        {
            SlotUI currentSlot = originalParent.GetComponent<SlotUI>();
            SwapSlotData(currentSlot, targetSlot);
        }
        else
        {
            // 드래그 종료 시 슬롯이 아닌 곳에 놓인 경우 원래 위치로 돌아감
            ReturnToOriginalPosition();
        }

        if (ShopUI.Instance != null)
        {
            var scrollRect = ShopUI.Instance.scrollRectTransform;

            if (RectTransformUtility.RectangleContainsScreenPoint(scrollRect, eventData.position))
            {
                var viewport = ShopUI.Instance.scrollViewport;

                if (viewport != null &&
                    RectTransformUtility.RectangleContainsScreenPoint(viewport, eventData.position, ShopUI.Instance.GetComponentInParent<Canvas>().worldCamera))
                {
                    // isSellable이 true인 경우에만 판매 팝업 띄움
                    if (itemData != null && itemData.isSellable)
                    {
                        ShopUI.Instance.sellPopup.Show(itemSlot);
                    }
                    return;
                }
            }
        }
    }

    private void SwapSlotData(SlotUI currentSlot, SlotUI targetSlot)
    {
        InventoryManager inventoryManager = InventoryManager.Instance;

        int currentSlotIndex = inventoryManager.slots.IndexOf(currentSlot.itemSlot);
        int targetSlotIndex = inventoryManager.slots.IndexOf(targetSlot.itemSlot);

        // 최신식 인덱스 교환
        (inventoryManager.slots[currentSlotIndex], inventoryManager.slots[targetSlotIndex]) = 
            (inventoryManager.slots[targetSlotIndex], inventoryManager.slots[currentSlotIndex]);
        
        // UI 아이템 오브젝트 스왑
        GameObject currentItem = currentSlot.item;
        GameObject targetItem = targetSlot.item;

        currentSlot.item = targetItem;
        targetSlot.item = currentItem;
        
        
        if (currentItem != null)
        {
            currentItem.transform.SetParent(targetSlot.transform);
            currentItem.transform.localPosition = Vector3.zero;
        }
        if (targetItem != null)
        {
            targetItem.transform.SetParent(currentSlot.transform);
            targetItem.transform.localPosition = Vector3.zero;
        }       

        // UI 새로고침
        UIManager.Instance.UpdateInventoryUI(); // 인벤토리 UI 업데이트
        UIManager.Instance.UpdateQuickSlotUI(); // 퀵슬롯 UI 업데이트
    }

    private void ReturnToOriginalPosition()
    {
        transform.SetParent(originalParent);
        transform.localPosition = Vector3.zero; // 원래 위치로 돌아감
    }
    #endregion
}