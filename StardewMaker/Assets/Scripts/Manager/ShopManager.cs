using UnityEngine;
using System.Collections.Generic;
using TMPro;
using NUnit.Framework.Interfaces;

public class ShopManager : MonoBehaviour
{
    public static ShopManager Instance { get; private set; }

    [Header("Shop Settings")]
    public List<ItemData> shopItems;
    public InventoryManager playerInventory;

    private void Awake()
    {
        // 싱글톤 설정
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        playerInventory = InventoryManager.Instance; // 플레이어 인벤토리 참조
    }

    // 아이템을 구매할 수 있는지 확인
    public bool CanAfford(ItemData item, int qty)
    {
        return InventoryManager.Instance.PlayerMoney >= item.buyPrice * qty;
    }
    
    // 아이템 구매 처리
    public bool Buy(ItemData item, int qty)
    {
        // 구매 가능한지 확인
        if (!CanAfford(item, qty))
        {
            return false;
        }

        // 인벤토리에 아이템 추가가 가능한지 확인
        bool itemAdded = playerInventory.AddItem(item, qty);

        // 아이템 추가 성공 여부 확인
        if (!itemAdded)
        {
            return false;
        }

        var prevMoney = InventoryManager.Instance.PlayerMoney;

        // 금액 차감
        InventoryManager.Instance.PlayerMoney -= item.buyPrice * qty;

        if (prevMoney != InventoryManager.Instance.PlayerMoney)
        {
            SoundManager.Instance.PlaySFX("Coin");
        }

        // UI 업데이트
        UIManager.Instance.UpdateInventoryUI();

        return true;
    }

    // 아이템 판매 처리
    public bool Sell(ItemData itemData, int qty)
    {
        Debug.Log(itemData);
        Debug.Log(qty);
        // 상점에서 취급하는 아이템인지 확인
        if (!itemData.isSellable)
        {
            return false;
        }

        // 인벤토리에 충분한 수량이 있는지 확인
        bool hasEnough = false;
        foreach (var slot in playerInventory.slots)
        {
            if (slot.itemData == itemData && slot.quantity >= qty)
            {
                hasEnough = true;
                break;
            }
        }

        if (!hasEnough)
        {
            return false;
        }

        playerInventory.RemoveItem(itemData, qty);

        var prevMoney = InventoryManager.Instance.PlayerMoney;
        InventoryManager.Instance.PlayerMoney += itemData.sellPrice * qty;

        if (prevMoney != InventoryManager.Instance.PlayerMoney)
        {
            SoundManager.Instance.PlaySFX("Coin");
        }

        // UI 업데이트
        UIManager.Instance.UpdateInventoryUI();

        return true;
    }

    // 특정 아이템의 판매 가격 반환
    public int GetSellPrice(ItemData itemData)
    {
        return shopItems.Contains(itemData) ? itemData.sellPrice : 0;
    }

    // 상점에 등록된 아이템인지 반환
    public ItemData GetShopItem(ItemData item)
    {
        return shopItems.Find(i => i == item);
    }
}