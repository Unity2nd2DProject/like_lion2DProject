using UnityEngine;
using System.Collections.Generic;
using TMPro;
using NUnit.Framework.Interfaces;
using System;

public class ShopManager : Singleton<ShopManager>
{
    // 아이템 구매 처리
    public bool Buy(ItemData item, int qty = 1)
    {
        // 구매 가능한지 확인
        if (!CanAfford(item, qty))
        {
            return false;
        }

        // 인벤토리에 아이템 추가가 가능한지 확인
        bool itemAdded = InventoryManager.Instance.AddItem(item, qty);

        // 아이템 추가 성공 여부 확인
        if (!itemAdded)
        {
            Debug.Log("구매 실패: 인벤토리에 공간이 부족합니다.");
            return false;
        }

        // 금액 차감
        InventoryManager.Instance.PlayerMoney -= item.buyPrice * qty;

        // UI 업데이트
        UIManager.Instance.UpdateInventoryUI();

        return true;
    }

    // 아이템 판매 처리
    public bool Sell(ItemData itemData, int qty = 1)
    {
        // 상점에서 취급하는 아이템인지 확인
        if (!itemData.isSellable)
        {
            return false;
        }

        // 인벤토리에 아이템 제거가 가능한지 확인
        bool hasEnough = InventoryManager.Instance.RemoveItem(itemData, qty);

        if (!hasEnough)
        {
            Debug.Log("판매 실패: 인벤토리에 아이템이 부족합니다.");
            return false;
        }

        // 금액 추가
        InventoryManager.Instance.PlayerMoney += itemData.sellPrice * qty;

        // UI 업데이트
        UIManager.Instance.UpdateInventoryUI();

        return true;
    }

    public bool CanAfford(ItemData item, int qty = 1)
    {
        return InventoryManager.Instance.PlayerMoney >= item.buyPrice * qty;
    }

    public void OpenShop(List<ItemData> itemsForSale)
    {
        UIManager.Instance.ShopUI.SetShopUI(itemsForSale);
        UIManager.Instance.OpenShopUI();
    }
}