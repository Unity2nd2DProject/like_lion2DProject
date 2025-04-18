using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class InventorySlotUI : MonoBehaviour, IBeginDragHandler, IEDragHandler, IEndDragHandler, IDropHandler
{
    private ItemData itemData;
    public Image icon;
    public TextMeshProUGUI quantityText;
    public Button slotButton;

    private int slotIndex;

    private void Awake()
    {
        //icon = GetComponentInChildren<Image>();
        //quantityText = GetComponentInChildren<TextMeshProUGUI>();
        //slotButton = GetComponentInChildren<Button>();

        slotButton.onClick.AddListener(OnSlotClicked);
    }

    public void SetSlotIndex(int index)
    {
        slotIndex = index;
    }

    public void SetSlot(ItemData _item, int quantity)
    {
        itemData = _item;
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
        if (itemData != null)
        {
            InventoryUI.Instance.selectedSlotIndex = slotIndex;
            //Debug.Log($"SelectedSlotIndex : {InventoryUI.Instance.selectedSlotIndex}");
        }
    }
}
