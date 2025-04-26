using System;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    [Header("Inventory and QuickSlot")]
    public GameObject InventoryUI;
    public GameObject QuickSlotUI;


    [Header("Stat UI")]
    [SerializeField] private StatUI statUIPrefab;
    [SerializeField] private Transform statUIParent;

    private StatUI statUIInstance;

    
    protected override void Awake()
    {
        base.Awake();
    }

    public void InitializeStatUI(List<Stat> stats)
    {
        statUIInstance = Instantiate(statUIPrefab, statUIParent);
        statUIInstance.Initialize(stats);
    }

    internal void UpdateQuickSlotUI()
    {
        throw new NotImplementedException();
    }

    internal void UpdateInventoryUI()
    {
        throw new NotImplementedException();
    }
}

