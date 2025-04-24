using System.Collections.Generic;
using UnityEngine;

public class UIManager : Singleton<UIManager>
{

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

}

