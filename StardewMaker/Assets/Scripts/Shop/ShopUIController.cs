using UnityEngine;
using TMPro;

public class ShopUIController : MonoBehaviour
{
    public Transform buySlotParent;
    public GameObject slotPrefab;
    public ShopManager shopManager;

    public ItemInfoPopupUI itemInfoPopup;

    public TextMeshProUGUI moneyText;

    void OnEnable()
    {
        UpdateUI();
    }

    public void UpdateUI()
    {
        moneyText.text = $"{shopManager.playerMoney} G";

        foreach (Transform child in buySlotParent)
            Destroy(child.gameObject);

        foreach (var item in shopManager.shopItems)
        {
            var slot = Instantiate(slotPrefab, buySlotParent);
            var ui = slot.GetComponent<ShopSlotUI>();
            ui.Setup(item, shopManager, itemInfoPopup, this);

        }
    }
}