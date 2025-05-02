using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;

public class DaughterManager : Singleton<DaughterManager>
{
    [Header("Daughter Status")]

    private List<Stat> stats = new List<Stat>();

    [SerializeField] private Stat moodStat;
    [SerializeField] private Stat vitalityStat;
    [SerializeField] private Stat hungerStat;
    [SerializeField] private Stat trustStat;


    protected override void Awake()
    {
        base.Awake();
        if (!isValid) return;

        Initialize();
    }

    private void Initialize()
    {
        moodStat.Initialize(StatType.MOOD, 100, 50);
        vitalityStat.Initialize(StatType.VITALITY, 100, 60);
        hungerStat.Initialize(StatType.HUNGER, 100, 80);
        trustStat.Initialize(StatType.TRUST, 100, 80);

        stats.Add(moodStat);
        stats.Add(vitalityStat);
        stats.Add(hungerStat);
        stats.Add(trustStat);

        // UIManager.Instance.InitializeStatUI(stats);
    }

    public List<Stat> GetStats()
    {
        return stats;
    }

    public void SetStats(StatType statType, int value)
    {
        foreach (var i in stats)
        {
            if (i.statType == statType)
            {
                i.CurrentValue = value;
            }
        }
    }
}
