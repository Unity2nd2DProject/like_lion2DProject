using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryUI : MonoBehaviour
{
    public static InventoryUI Instance;

    public GameObject inventoryPanel;

    public Inventory inventory;
    public InventorySlotUI[] slotUIs;
    public Button cancelButton;
    public TextMeshProUGUI money;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        inventoryPanel.SetActive(false);
        cancelButton.onClick.AddListener(OnCancelButtonClicked);
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
        inventoryPanel.SetActive(!inventoryPanel.activeSelf);
    }

    public void ShowInventory()
    {
        inventoryPanel.SetActive(true);
    }

    public void HideInventory()
    {
        inventoryPanel.SetActive(false);
    }
}
