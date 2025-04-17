using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventorySlotUI : MonoBehaviour
{
    private ItemData item;
    public Image icon;
    public TextMeshProUGUI quantityText;
    public Button slotButton;
    public Button interactionButton;

    private int slotIndex;

    private void Awake()
    {
        //icon = GetComponentInChildren<Image>();
        //quantityText = GetComponentInChildren<TextMeshProUGUI>();
        //slotButton = GetComponentInChildren<Button>();

        slotButton.onClick.AddListener(OnSlotClicked);
        interactionButton.onClick.AddListener(OnInteractionClicked);
    }

    public void SetSlotIndex(int index)
    {
        slotIndex = index;
    }

    public void SetSlot(ItemData _item, int quantity)
    {
        item = _item;
        if (_item != null)
        {
            icon.enabled = true;
            icon.sprite = _item.icon;
            quantityText.text = _item.isStackable ? quantity.ToString() : "";
        }
        else
        {
            icon.enabled = false;
            quantityText.text = "";
        }
    }

    private void OnSlotClicked()
    {
        if (item != null)
        {
            interactionButton.gameObject.SetActive(true);
            InventoryUI.Instance.selectedSlotIndex = slotIndex;

            if (item.itemType == ItemType.Seed)
            {
                interactionButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Plant";
            }

            //Debug.Log($"SelectedSlotIndex : {InventoryUI.Instance.selectedSlotIndex}");
        }
    }

    private void OnInteractionClicked()
    {
        if (item.itemType == ItemType.Seed)
        {
            if (PlayerController.Instance.Plant(item.cropToGrow))
            {
                Inventory.Instance.RemoveItem(item);
                InventoryUI.Instance.selectedSlotIndex = -1;
                item = null;
            }

        }
    }

}
