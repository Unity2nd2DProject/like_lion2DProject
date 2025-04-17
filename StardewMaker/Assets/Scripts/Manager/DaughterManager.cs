using UnityEngine;
using System.Collections.Generic;

public class DaughterManager : Singleton<DaughterManager>
{
    [Header("Daughter Status")]
    private List<Stat> stats = new List<Stat>();
    

    protected override void Awake()
    {
        base.Awake();

        Initialize();
    }

    private void Initialize()
    {
        stats.Add(new Stat(StatType.Mood, 100 ,50));
        stats.Add(new Stat(StatType.Vitality, 100, 60));
        stats.Add(new Stat(StatType.Hunger, 100, 80));
        stats.Add(new Stat(StatType.Trust, 100, 80));

        UIManager.Instance.InitializeStatUI(stats);
    }
}
