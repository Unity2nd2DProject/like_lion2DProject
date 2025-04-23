using UnityEngine;
using System.Collections.Generic;

public class ShopManager : MonoBehaviour
{
    public List<ShopItemData> shopItems;
    public Inventory playerInventory;
    public int playerMoney;

    public bool CanAfford(ShopItemData item, int qty)
    {
        return playerMoney >= item.buyPrice * qty;
    }

    public bool Buy(ShopItemData item, int qty)
    {
        if (!CanAfford(item, qty))
        {
            return false;
        }

        if (!playerInventory.AddItem(item.itemData, qty))
        {
            return false;
        }

        playerMoney -= item.buyPrice * qty;
        return true;
    }

    public bool Sell(ItemData itemData, int qty)
    {
        var shopItem = shopItems.Find(i => i.itemData == itemData);

        if (shopItem == null)
            return false;

        if (!playerInventory.slots.Exists(s => s.itemData == itemData && s.quantity >= qty))
            return false;

        playerInventory.RemoveItem(itemData, qty);
        playerMoney += shopItem.sellPrice * qty;
        return true;
    }

    public int GetSellPrice(ItemData itemData)
    {
        var shopItem = shopItems.Find(i => i.itemData == itemData);
        return shopItem != null ? shopItem.sellPrice : 0;
    }
}