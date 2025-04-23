using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using NUnit.Framework.Interfaces;

public class SellSlotUI : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public Image icon;
    public TextMeshProUGUI quantityText;

    private ShopItemData itemData;
    private ItemSlot slot;
    private SellPopupUI popup;
    private ItemInfoPopupUI infoPopup;

    public void SetSlot(ItemSlot slotData, SellPopupUI popupUI)
    {
        slot = slotData;
        popup = popupUI;

        icon.sprite = slot.itemData.icon;
        quantityText.text = slot.itemData.isStackable ? $"x{slot.quantity}" : "";
    }

    private float lastClickTime;
    private const float doubleClickThreshold = 0.3f;

    public void OnPointerClick(PointerEventData eventData)
    {
        float time = Time.unscaledTime;

        if (time - lastClickTime < doubleClickThreshold)
        {
            popup.Show(slot);
        }

        lastClickTime = time;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        infoPopup.Show(itemData);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        infoPopup.Hide();
    }
}