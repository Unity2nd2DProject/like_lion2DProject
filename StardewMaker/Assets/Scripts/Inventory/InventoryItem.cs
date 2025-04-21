using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using System;


public class InventoryItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private ItemData itemData;
    public Image icon;
    public TextMeshProUGUI quantityText;
    public Button slotButton;

    private Transform originalParent;

    private Canvas canvas;
    private CanvasGroup canvasGroup;

    private void Awake()
    {
        canvas = GetComponentInParent<Canvas>();
        canvasGroup = GetComponent<CanvasGroup>();
        slotButton.onClick.AddListener(OnSlotClicked);
    }

    public void SetSlot(ItemData _item, int quantity)
    {
        itemData = _item;
        if (_item != null)
        {
            icon.enabled = true;
            icon.sprite = _item.icon;
            quantityText.text = _item.isStackable ? quantity.ToString() : "";
        }
        else
        {
            icon.enabled = false;
            quantityText.text = "";
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

    #region Drag & Drop
    public void OnBeginDrag(PointerEventData eventData)
    {
        originalParent = transform.parent; // 드래그 시작 슬롯 저장

        if (itemData == null)
        {
            return;
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

        InventorySlotUI targetSlot = dropTarget.GetComponentInParent<InventorySlotUI>();
        if (targetSlot != null)
        {
            InventorySlotUI currentSlot = originalParent.GetComponent<InventorySlotUI>();
            SwapSlotData(currentSlot, targetSlot);
        }
        else
        {
            // 드래그 종료 시 슬롯이 아닌 곳에 놓인 경우 원래 위치로 돌아감
            ReturnToOriginalPosition();
        }
    }
    private void SwapSlotData(InventorySlotUI currentSlot, InventorySlotUI targetSlot)
    {
        Inventory inventory = InventoryUI.Instance.inventory;

        int currentIndex = InventoryUI.Instance.inventorySlotUIs.IndexOf(currentSlot);
        int targetIndex = InventoryUI.Instance.inventorySlotUIs.IndexOf(targetSlot);

        InventorySlot temp = inventory.slots[currentIndex];
        inventory.slots[currentIndex] = inventory.slots[targetIndex];
        inventory.slots[targetIndex] = temp;

        // UI도 스왑
        GameObject currentItem = currentSlot.item;
        GameObject targetItem = targetSlot.item;

        currentSlot.item = targetItem;
        targetSlot.item = currentItem;

        currentItem.transform.SetParent(targetSlot.transform);
        currentItem.transform.localPosition = Vector3.zero;

        targetItem.transform.SetParent(currentSlot.transform);
        targetItem.transform.localPosition = Vector3.zero;


        InventoryUI.Instance.UpdateInventoryUI();
    }

    private void ReturnToOriginalPosition()
    {
        transform.SetParent(originalParent);
        transform.localPosition = Vector3.zero; // 원래 위치로 돌아감
    }
    #endregion

}



