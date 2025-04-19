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

    private Vector3 originalPosition;


    private void Awake()
    {
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
        if (itemData == null)
        {
            return;
        }

        originalPosition = transform.position; // 드래그 시작 위치 저장
        transform.SetParent(transform.root);
        transform.SetAsLastSibling(); // 드래그한 아이템을 가장 위로 올림
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (eventData.pointerCurrentRaycast.gameObject == null || eventData.pointerCurrentRaycast.gameObject.GetComponent<InventorySlotUI>() == null)
        {
            transform.position = originalPosition;
        }
        else
        {
            InventorySlotUI targetSlot = eventData.pointerCurrentRaycast.gameObject.GetComponent<InventorySlotUI>();
            SwapItems(targetSlot);
        }
    }

    private void SwapItems(InventorySlotUI targetSlot)
    {

    }
    #endregion

}



