using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class InventoryUI : MonoBehaviour, IDropHandler
{
    public static InventoryUI Instance;
    public GameObject inventorGrid;
    public Inventory inventory;
    public InventorySlotUI[] slotUIs;
    public Button cancelButton;
    public TextMeshProUGUI money;
    public int selectedSlotIndex = -1;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        inventorGrid.SetActive(false);
        cancelButton.onClick.AddListener(OnCancelButtonClicked);

        for (int i = 0; i < slotUIs.Length; i++)
        {
            slotUIs[i].SetSlotIndex(i);
        }
    }

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

    private void OnCancelButtonClicked()
    {
        ToggleInventory();
    }

    public void ToggleInventory()
    {
        inventorGrid.SetActive(!inventorGrid.activeSelf);
    }

    public void ShowInventory()
    {
        inventorGrid.SetActive(true);
    }

    public void HideInventory()
    {
        inventorGrid.SetActive(false);
    }

    public ItemData GetSelectedItem()
    {
        if (selectedSlotIndex >= 0 && selectedSlotIndex < inventory.slots.Count)
        {
            return inventory.slots[selectedSlotIndex].itemData;
        }
        return null;
    }
}
