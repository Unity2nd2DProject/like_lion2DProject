using System.Collections.Generic;
using UnityEngine;

public class SellUIController : MonoBehaviour
{
    public Inventory inventory;
    public ShopManager shopManager;
    public ShopUIController shopUIController;
    public ItemInfoPopupUI itemInfoPopupUI;

    public Transform sellSlotParent;
    public GameObject sellSlotPrefab;
    public SellPopupUI sellPopup;

    public bool testMode = true; // <- 테스트 모드용 체크박스

    void Start()
    {
        sellPopup.Init(shopManager, shopUIController);
    }

    void OnEnable()
    {
        UpdateSellUI();
    }

    public void UpdateSellUI()
    {
        foreach (Transform child in sellSlotParent)
            Destroy(child.gameObject);

        foreach (var slot in inventory.slots)
        {
            if (!slot.IsEmpty())
            {
                Debug.Log($"[SellSlot] 슬롯 생성됨: {slot.itemData.itemName} x{slot.quantity}");

                var go = Instantiate(sellSlotPrefab, sellSlotParent);
                var ui = go.GetComponent<SellSlotUI>();
                ui.SetSlot(slot, sellPopup);
            }
        }
    }
}