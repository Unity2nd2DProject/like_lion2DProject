using System.Collections.Generic;
using UnityEngine;

public class QuickSlotManager : Singleton<QuickSlotManager>
{
    public int quickSlotSize = 10;
    public List<ItemSlot> slots = new List<ItemSlot>();

    public ItemSlot currentSelect;

    protected override void Awake()
    {
        base.Awake();

        SetQuickSlot();
    }

    public void SetQuickSlot()
    {
        for (int i = 0; i < quickSlotSize; i++)
        {
            slots.Add(new ItemSlot()); // 슬롯 초기화
        }
    }
}
