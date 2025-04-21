using System.Collections.Generic;
using UnityEngine;

public class QuickSlotUI : Singleton<QuickSlotUI>
{
    QuickSlotManager quickSlotManager;

    public List<QuickSlotSlotUI> quickSlotSlotUIs = new List<QuickSlotSlotUI>();

    protected override void Awake()
    {
        base.Awake();
        quickSlotManager = QuickSlotManager.Instance;

        UpdateUI();
    }

    public void UpdateUI()
    {
        for (int i = 0; i < quickSlotManager.quickSlotSize; i++)
        {
            quickSlotSlotUIs[i].UpdateSlot(quickSlotManager.slots[i]);
        }
    }
}
