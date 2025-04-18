using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemInfoPopupUI : MonoBehaviour
{
    public Image icon;
    public TextMeshProUGUI itemNameText;
    public TextMeshProUGUI descriptionText;

    public void Show(ShopItemData item)
    {
        if (item == null)
        {
            Hide();
            return;
        }

        icon.sprite = item.itemData.icon;
        itemNameText.text = item.itemData.itemName;
        descriptionText.text = GetItemDescription(item);

        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    string GetItemDescription(ShopItemData item)
    {
        return item.itemDescription;
    }
}