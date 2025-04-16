using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventorySlotUI : MonoBehaviour
{
    public Image icon;
    public TextMeshProUGUI quantityText;

    public void SetSlot(ItemData item, int quantity)
    {
        if (item != null)
        {
            icon.enabled = true;
            icon.sprite = item.icon;
            quantityText.text = item.isStackable ? quantity.ToString() : "";
        }
        else
        {
            icon.enabled = false;
            quantityText.text = "";
        }
    }

}
