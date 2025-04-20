using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using System;


public class InventoryItem : MonoBehaviour,IBeginDragHandler, IDragHandler, IEndDragHandler
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

        Debug.Log("OnEndDrag 호출됨");
        if (itemData == null)
        {
            Debug.Log("드래그 종료: itemData가 null입니다.");

            ReturnToOriginalPosition();
            return;
        }
        GameObject dropTarget = eventData.pointerEnter;

        if (dropTarget == null)
        {
            Debug.Log("드래그 종료: dropTarget이 null입니다.");
            ReturnToOriginalPosition();
            return;
        }
        Debug.Log($"드롭 대상 객체 이름: {dropTarget.name}");

        InventorySlotUI targetSlot = dropTarget.GetComponentInParent<InventorySlotUI>();
        if(targetSlot != null)
        {
            Debug.Log("드롭 대상이 슬롯입니다. 스왑 시도합니다.");
            InventorySlotUI currentSlot = originalParent.GetComponent<InventorySlotUI>();

            SwapSlotData(currentSlot, targetSlot);

        }
        else
        {
            // 드래그 종료 시 슬롯이 아닌 곳에 놓인 경우 원래 위치로 돌아감
            Debug.Log("드롭 대상이 슬롯이 아닙니다. 원위치로 복귀합니다.");
            ReturnToOriginalPosition();
        }
    }
    private void SwapSlotData(InventorySlotUI currentSlot, InventorySlotUI targetSlot)
    {
        var inventory = InventoryUI.Instance.inventory;

        int currentIndex = InventoryUI.Instance.inventorySlotUIs.IndexOf(currentSlot);
        int targetIndex = InventoryUI.Instance.inventorySlotUIs.IndexOf(targetSlot);

        InventorySlot temp = inventory.slots[currentIndex];
        inventory.slots[currentIndex] = inventory.slots[targetIndex];
        inventory.slots[targetIndex] = temp;

        Debug.Log($"[SwapSlotData] 인벤토리 슬롯 스왑: {currentIndex} <-> {targetIndex}");

        // UI도 스왑
        GameObject currentItem = currentSlot.item;
        GameObject targetItem = targetSlot.item;

        currentSlot.item = targetItem;
        targetSlot.item = currentItem;

        if (currentItem != null)
        {
            currentItem.transform.SetParent(targetSlot.transform);
            currentItem.transform.localPosition = Vector3.zero;
            Debug.Log($"[SwapSlotData] {currentItem.name} → {targetSlot.name}");
        }

        if (targetItem != null)
        {
            targetItem.transform.SetParent(currentSlot.transform);
            targetItem.transform.localPosition = Vector3.zero;
            Debug.Log($"[SwapSlotData] {targetItem.name} → {currentSlot.name}");
        }

        InventoryUI.Instance.UpdateInventoryUI();
    }

    private void ReturnToOriginalPosition()
    {
        transform.SetParent(originalParent);
        transform.localPosition = Vector3.zero; // 원래 위치로 돌아감
    }
    #endregion

}



