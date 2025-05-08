using UnityEngine;
using UnityEngine.EventSystems;

public class SlotUI : MonoBehaviour, IDropHandler
{
    public GameObject item;
    [HideInInspector]
    public ItemSlot itemSlot;

    private bool isSellMode = false;
    private SellPopupUI sellPopup;

    public void UpdateSlot(ItemSlot inventorySlot)
    {
        if (inventorySlot != null)
        {
            this.itemSlot = inventorySlot;
        }

        if (item != null && inventorySlot != null)
            item.GetComponent<SlotedItemUI>().SetSlot(inventorySlot.itemData, inventorySlot.quantity, inventorySlot);
    }

    public void SetSellMode(bool enable, SellPopupUI popup = null)
    {
        isSellMode = enable;
        sellPopup = popup;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (isSellMode && itemSlot != null && itemSlot.itemData != null)
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
