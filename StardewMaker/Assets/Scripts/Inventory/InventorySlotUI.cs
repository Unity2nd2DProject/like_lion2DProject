using UnityEngine;
using UnityEngine.EventSystems;

public class InventorySlotUI : SlotUI, IDropHandler
{
    private void Awake()
    {
        slotType = SlotType.Inventory;
    }
}