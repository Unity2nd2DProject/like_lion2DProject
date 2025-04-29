using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class ShopManager : MonoBehaviour
{
    public static ShopManager Instance { get; private set; }

    [Header("Shop Settings")]
    public List<ItemData> shopItems;
    public InventoryManager playerInventory;

    [Header("Player Money")]
    public int playerMoney;
    public TextMeshProUGUI moneyText;  // Inspector에 드래그할 머니 표시용 UI

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
        UpdateMoneyUI();  // 처음 한 번 초기화
        playerInventory = InventoryManager.Instance; // 플레이어 인벤토리 참조
    }

    // 머니가 바뀔 때마다 이 함수를 호출
    private void UpdateMoneyUI()
    {
        if (moneyText != null)
        {
            moneyText.text = playerMoney.ToString("#,0");
        }
    }

    // 아이템을 구매할 수 있는지 확인
    public bool CanAfford(ItemData item, int qty)
    {
        return playerMoney >= item.buyPrice * qty;
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

        // 금액 차감
        playerMoney -= item.buyPrice * qty;

        // UI 업데이트
        UIManager.Instance.UpdateInventoryUI();
        UpdateMoneyUI();

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
        playerMoney += itemData.sellPrice * qty;

        // UI 업데이트
        UIManager.Instance.UpdateInventoryUI();
        UpdateMoneyUI();

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