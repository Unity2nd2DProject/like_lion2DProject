using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    public Inventory inventory;
    public InventorySlotUI[] slotUIs;

    private void Update()
    {
        for(int i=0; i<slotUIs.Length; i++)
        {
            if (i < inventory.slots.Count)
            {
                var slot = inventory.slots[i];
                slotUIs[i].SetSlot(slot.itemData, slot.quantity);
            }
        }
    }
}
