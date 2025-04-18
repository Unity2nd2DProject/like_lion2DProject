using UnityEngine;
using System.Collections.Generic;

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

        Initialize();
    }

    private void Initialize()
    {
        moodStat.Initialize(StatType.Mood, 100 ,50);
        vitalityStat.Initialize(StatType.Vitality, 100, 60);
        hungerStat.Initialize(StatType.Hunger, 100, 80);
        trustStat.Initialize(StatType.Trust, 100, 80);

        stats.Add(moodStat);
        stats.Add(vitalityStat);
        stats.Add(hungerStat);
        stats.Add(trustStat);

        UIManager.Instance.InitializeStatUI(stats);
    }
}
