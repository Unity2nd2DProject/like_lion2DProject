using UnityEngine;
using UnityEngine.EventSystems;

public class InventorySlotUI : MonoBehaviour, IDropHandler
{
    public GameObject item;
    [HideInInspector]
    public InventorySlot InventorySlot;

    public void SetSlot(InventorySlot inventorySlot)
    {
        if (inventorySlot != null)
        {
            this.InventorySlot = inventorySlot;
        }

        item.GetComponent<InventoryItem>().SetSlot(InventorySlot.itemData, InventorySlot.quantity);
    }

    public void OnDrop(PointerEventData eventData)
    {

    }
}