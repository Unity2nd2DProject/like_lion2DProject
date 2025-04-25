using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class IngredientSlotUI : MonoBehaviour
{
    private ItemData itemData;
    public Image icon;
    public TextMeshProUGUI quantityText;
    public Button slotButton;

    private Transform originalParent;

    private Canvas canvas;
    private CanvasGroup canvasGroup;

    private void Awake()
    {

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

    public ItemData GetItemData()
    {
        return itemData;
    }

    private void OnSlotClicked()
    {
        if (itemData != null)
        {
            // TODO: 아이템 클릭 시 행동 정의
        }
    }
}




