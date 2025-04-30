using NUnit.Framework.Interfaces;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : Singleton<InventoryManager>
{
    public int inventorySize = 25;
    public int quickSlotSize = 10;
    public List<ItemSlot> slots = new List<ItemSlot>();

    public int currentSelectedQuickSlotIndex;

    public List<ItemData> starterItems = new List<ItemData>(); // 테스트를 위해 시작 아이템 추가 

    protected override void Awake()
    {
        base.Awake();

        InitializeInventory();
    }

    private void Update()
    {
        GetKeyborardNumber();
        GetMouseScroll();
    }

    private void InitializeInventory()
    {
        for (int i = 0; i < inventorySize + quickSlotSize; i++)
        {
            slots.Add(new ItemSlot());
        }
        currentSelectedQuickSlotIndex = 0;
        UIManager.Instance.InitializeInventoryAndQuickSlot();

        // 테스트 용 아이템 추가
        for (int i = 0; i < starterItems.Count; i++)
        {
            AddItem(starterItems[i], 4);
        }        
    }

    public bool AddItem(ItemData newItem, int amount = 1)
    {
        if (newItem.isStackable)
        {
            foreach (var inventorySlot in slots)
            {
                if (inventorySlot.itemData == newItem) // 같은 아이템이 있는 슬롯을 찾음
                {
                    inventorySlot.quantity += amount; // 수량 증가
                    UIManager.Instance.UpdateInventoryUI();
                    return true; // 아이템 추가 완료
                }
            }
            foreach (var inventorySlot in slots)
            {
                if (inventorySlot.IsEmpty()) // 비어있는 슬롯을 찾음
                {
                    inventorySlot.itemData = newItem; // 아이템 할당
                    inventorySlot.quantity = amount; // 수량 설정
                    UIManager.Instance.UpdateInventoryUI();
                    return true; // 아이템 추가 완료
                }
            }
        }
        else
        {
            foreach (var inventorySlot in slots)
            {
                if (inventorySlot.IsEmpty()) // 비어있는 슬롯을 찾음
                {
                    inventorySlot.itemData = newItem; // 아이템 할당
                    inventorySlot.quantity = amount; // 수량 설정
                    UIManager.Instance.UpdateInventoryUI();
                    return true; // 아이템 추가 완료
                }
            }
        }
        UIManager.Instance.UpdateInventoryUI();
        return false; // 슬롯이 부족하여 아이템 추가 실패
    }

    public bool RemoveItem(ItemData item, int amount = 1)
    {
        foreach (var slot in slots)
        {
            if (slot.itemData == item)
            {
                if (slot.quantity >= amount)
                {
                    slot.quantity -= amount;

                    if (slot.IsEmpty()) // 수량이 0 이하가 되면 슬롯을 비움
                    {
                        slot.Clear();
                    }
                    UIManager.Instance.UpdateInventoryAndQuickSlot();
                    return true; // 아이템 제거 성공
                }
            }
        }
        UIManager.Instance.UpdateInventoryAndQuickSlot();
        return false; // 아이템이 없거나 수량 부족
    }

    public ItemData GetItem(string itemName)
    {
        foreach (var slot in slots)
        {
            if (!slot.IsEmpty() && slot.itemData.itemName == itemName)
            {
                return slot.itemData;
            }
        }

        return null;
    }

    public bool CheckItem(ItemData item)
    {
        foreach (var slot in slots)
        {
            if (slot.itemData == item)
            {
                return true; // 아이템이 존재함
            }
        }
        return false; // 아이템이 존재하지 않음
    }

    public bool isFull()
    {
        foreach (var slot in slots)
        {
            if (slot.IsEmpty())
            {
                return false; // 비어있는 슬롯이 있음
            }
        }
        return true; // 모든 슬롯이 가득 참
    }

    #region 퀵슬롯 입력
    private void GetKeyborardNumber()
    {
        for (int i = 0; i < 10; i++)
        {
            int keyNumber = (i + 1) % 10;
            if (Input.GetKeyDown((KeyCode)((int)KeyCode.Alpha0 + keyNumber)))
            {
                SetSelectedSlot(i);
            }
        }
    }

    private void GetMouseScroll()
    {
        // 상점 UI가 켜져 있으면 스크롤 무시
        if (ShopUI.Instance != null && ShopUI.Instance.gameObject.activeSelf)
        {
            return;
        }

        float scroll = Input.GetAxis("Mouse ScrollWheel");
        int selectedIndex = currentSelectedQuickSlotIndex;

        if (scroll > 0f) // 휠 위로
        {
            selectedIndex = (selectedIndex - 1 + quickSlotSize) % quickSlotSize;
            SetSelectedSlot(selectedIndex);
        }
        else if (scroll < 0f) // 휠 아래로
        {
            selectedIndex = (selectedIndex + 1) % quickSlotSize;
            SetSelectedSlot(selectedIndex);
        }
    }
    #endregion

    private void SetSelectedSlot(int index)
    {
        currentSelectedQuickSlotIndex = index;
        UIManager.Instance.UpdateQuickSlotUI(); // UI 갱신
    }

    public ItemData GetQuickSlotCurrentSelectedItem()
    {
        return slots[currentSelectedQuickSlotIndex + inventorySize].itemData;
    }
}
