using UnityEngine;
using UnityEngine.EventSystems;

public class InventorySlotUI : MonoBehaviour, IDropHandler
{
    public InventoryItem item;

    public void SetSlot(ItemData _item, int quantity)
    {
        item.SetSlot(_item, quantity);
    }

    public void OnDrop(PointerEventData eventData)
    {
        GameObject dropped = eventData.pointerDrag;

        if (dropped != null)
        {
            // 부모를 현재 슬롯으로 변경
            dropped.transform.SetParent(transform);
            dropped.transform.localPosition = Vector3.zero;
        }
    }
}