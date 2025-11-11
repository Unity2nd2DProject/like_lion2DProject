using UnityEngine;
using UnityEngine.EventSystems;

public class ItemSlotUI : MonoBehaviour
{
    public GameObject item;
    [HideInInspector] public ItemSlot itemSlot;

    public void UpdateSlot(ItemSlot inventorySlot)
    {
        if (inventorySlot != null)
        {
            this.itemSlot = inventorySlot;
        }

        if (item != null && inventorySlot != null)
            item.GetComponent<SlotedItemUI>().SetSlot(inventorySlot.itemData, inventorySlot.quantity, inventorySlot);
    }
}
