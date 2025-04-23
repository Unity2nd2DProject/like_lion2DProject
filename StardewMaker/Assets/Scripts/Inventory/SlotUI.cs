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

    public void UpdateSlot(ItemSlot inventorySlot)
    {
        if (inventorySlot != null)
        {
            this.itemSlot = inventorySlot;
        }

        item.GetComponent<SlotedItemUI>().SetSlot(itemSlot.itemData, itemSlot.quantity);
    }

    public void OnDrop(PointerEventData eventData)
    {

    }
}
