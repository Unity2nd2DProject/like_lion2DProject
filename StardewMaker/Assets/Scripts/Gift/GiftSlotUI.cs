using NUnit.Framework.Interfaces;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class GiftSlotUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private ItemData itemData;
    public Image icon;
    public TextMeshProUGUI quantityText;
    public Button slotButton;
    public GameObject quantityTextBox;

    private Transform originalParent;

    private Canvas canvas;
    private CanvasGroup canvasGroup;

    public void SetSlot(ItemData itemData, int quantity)
    {
        this.itemData = itemData;
        if (itemData != null)
        {
            icon.enabled = true;
            icon.sprite = itemData.icon;
            quantityText.text = itemData.isStackable ? quantity.ToString() : "";

            if (itemData.isStackable)
            {
                quantityTextBox.SetActive(true);
            }
        }
        else
        {
            icon.enabled = false;
            quantityText.text = "";
            quantityTextBox.SetActive(false);
        }
    }

    public ItemData GetItemData()
    {
        return itemData;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (itemData != null)
        {
            UIManager.Instance.ShowTooltip(itemData, transform.position);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        UIManager.Instance.HideTooltip();
    }

    public void SetButton(GiftUI giftUI)
    {
        slotButton.onClick.AddListener(() =>
        {
            if (itemData != null)
            {
                giftUI.SetGift(itemData); // 선물 UI에 선물 설정
            }

        });
    }
}