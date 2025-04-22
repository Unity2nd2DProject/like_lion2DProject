using System;
using System.Collections.Generic;
using UnityEngine;

public class QuickSlotUI : Singleton<QuickSlotUI>
{
    QuickSlotManager quickSlotManager;

    public List<QuickSlotSlotUI> quickSlotSlotUIs = new List<QuickSlotSlotUI>();

    public GameObject currentSelectedCursor;

    protected override void Awake()
    {
        base.Awake();
        quickSlotManager = QuickSlotManager.Instance;

        //UpdateUI();
    }

    private void Start()
    {
        UpdateUI();
    }

    public void UpdateUI()
    {
        Debug.Log($"quickSlotSize : {quickSlotManager.quickSlotSize}");
        Debug.Log($"quickSlotManager.slots.Count : {quickSlotManager.slots.Count}");
        for (int i = 0; i < quickSlotManager.quickSlotSize; i++)
        {
            quickSlotSlotUIs[i].UpdateSlot(quickSlotManager.slots[i]);
        }
    }

    internal void UpdateSelectedSlot()
    {
        currentSelectedCursor.transform.SetParent(quickSlotSlotUIs[quickSlotManager.currentSelectedIndex].transform);
        currentSelectedCursor.transform.localPosition = new Vector3(-50, 50);
    }
}
