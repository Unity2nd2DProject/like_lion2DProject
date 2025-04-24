using UnityEngine;
using UnityEngine.EventSystems;

public enum SlotType
{
    Inventory,
    QuickSlot,
}

public class SlotUI : MonoBehaviour, IDropHandler
{
    public GameObject item;
    [HideInInspector]
    public ItemSlot itemSlot;
    public SlotType slotType;

    private bool isSellMode = false;
    private SellPopupUI sellPopup;

    public void UpdateSlot(ItemSlot inventorySlot)
    {
        if (inventorySlot != null)
        {
            this.itemSlot = inventorySlot;
        }

        item.GetComponent<SlotedItemUI>().SetSlot(inventorySlot.itemData, inventorySlot.quantity, inventorySlot);
    }

    public void SetSellMode(bool enable, SellPopupUI popup = null)
    {
        isSellMode = enable;
        sellPopup = popup;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (isSellMode && slotType == SlotType.Inventory && itemSlot != null && itemSlot.itemData != null)
        {
            if (itemSlot.itemData.isSellable)
            {
                sellPopup.Show(itemSlot);
            }
        }
    }

    public void OnDrop(PointerEventData eventData)
    {

    }
}
