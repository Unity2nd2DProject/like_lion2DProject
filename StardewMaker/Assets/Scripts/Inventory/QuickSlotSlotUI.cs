using UnityEngine;
using UnityEngine.EventSystems;

public class QuickSlotSlotUI : SlotUI, IDropHandler
{
    private void Awake()
    {
        slotType = SlotType.QuickSlot;
    }
}
