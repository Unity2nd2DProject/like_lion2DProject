using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class ShopSlotUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Image icon;
    public TextMeshProUGUI itemNameText, priceText;
    public Button buyButton;

    private ShopItemData itemData;
    private ShopManager shopManager;
    private ItemInfoPopupUI infoPopup;
    private ShopUIController uiController;

    public void Setup(ShopItemData data, ShopManager manager, ItemInfoPopupUI popup, ShopUIController controller)
    {
        itemData = data;
        shopManager = manager;
        infoPopup = popup;
        uiController = controller;

        icon.sprite = data.itemData.icon;
        itemNameText.text = data.itemData.itemName;
        priceText.text = $"{data.buyPrice} G";

        buyButton.onClick.RemoveAllListeners();
        buyButton.onClick.AddListener(OnBuyButtonClicked);
    }

    void OnBuyButtonClicked()
    {
        bool success = shopManager.Buy(itemData, 1);
        if (success)
        {
            uiController.UpdateUI();
        }
        else
        {
            Debug.Log("구매 실패: 돈이 부족하거나 인벤토리가 가득 찼습니다.");
        }
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
