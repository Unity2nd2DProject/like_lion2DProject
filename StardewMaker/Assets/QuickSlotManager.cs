using System;
using System.Collections.Generic;
using UnityEngine;

public class QuickSlotManager : Singleton<QuickSlotManager>
{
    public int quickSlotSize = 10;
    public List<ItemSlot> slots = new List<ItemSlot>();

    public int currentSelectedIndex;

    protected override void Awake()
    {
        base.Awake();

        SetQuickSlot();
        currentSelectedIndex = 0; // 초기 선택 인덱스 설정
    }

    private void Update()
    {
        GetKeyborardNumber();
        GetMouseScroll();
    }

    private void GetMouseScroll()
    {
        // 상점 UI가 켜져 있으면 스크롤 무시
        if (ShopUI.Instance != null && ShopUI.Instance.gameObject.activeSelf)
        {
            return;
        }

        float scroll = Input.GetAxis("Mouse ScrollWheel");
        int selectedIndex = currentSelectedIndex;

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

    public void SetQuickSlot()
    {
        for (int i = 0; i < quickSlotSize; i++)
        {
            slots.Add(new ItemSlot()); // 슬롯 초기화
        }
    }

    private void SetSelectedSlot(int index)
    {
        currentSelectedIndex = index;
        QuickSlotUI.Instance.UpdateSelectedSlot(); // UI 갱신
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

                    if (slot.quantity <= 0)
                    {
                        slot.itemData = null;
                        slot.quantity = 0;
                    }
                    QuickSlotUI.Instance.UpdateUI();
                    return true;
                }
            }
        }

        return false;
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
}
