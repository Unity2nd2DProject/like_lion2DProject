using UnityEngine;
using System.Collections.Generic;
using System;

public class DaughterManager : Singleton<DaughterManager>
{
    [Header("Daughter Status")]

    private List<Stat> stats = new List<Stat>();

    [SerializeField] private Stat moodStat;
    [SerializeField] private Stat vitalityStat;
    [SerializeField] private Stat hungerStat;
    [SerializeField] private Stat trustStat;

    [SerializeField] private Stat physicalStat;
    [SerializeField] private Stat musicStat;
    [SerializeField] private Stat artStat;
    [SerializeField] private Stat socialStat;
    [SerializeField] private Stat academicStat;
    [SerializeField] private Stat domesticStat;

    public static event Action<string> OnStatChangeRequested;

    public Dictionary<ConditionType, string> conditionStrDic = new(){
        {ConditionType.MOOD, "기분"},
        {ConditionType.VITALITY, "건강"},
        {ConditionType.HUNGER, "배고픔"},
        {ConditionType.TRUST, "신뢰도"},

        {ConditionType.PYSICAL, "운동"},
        {ConditionType.MUSIC, "음악"},
        {ConditionType.ART, "미술"},
        {ConditionType.SOCIAL, "사교"},
        {ConditionType.ACADEMIC, "학문"},
        {ConditionType.DOMESTIC, "생활"},
    };


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

        physicalStat.Initialize(StatType.PYSICAL, 250, 0);
        musicStat.Initialize(StatType.MUSIC, 250, 0);
        artStat.Initialize(StatType.ART, 250, 0);
        socialStat.Initialize(StatType.SOCIAL, 250, 0);
        academicStat.Initialize(StatType.ACADEMIC, 250, 0);
        domesticStat.Initialize(StatType.DOMESTIC, 250, 0);

        stats.Add(moodStat);
        stats.Add(vitalityStat);
        stats.Add(hungerStat);
        stats.Add(trustStat);

        stats.Add(physicalStat);
        stats.Add(musicStat);
        stats.Add(artStat);
        stats.Add(socialStat);
        stats.Add(academicStat);
        stats.Add(domesticStat);

        UIManager.Instance.InitializeStatUI(stats);
    }

    private void Start()
    {
        SaveManager.Instance.LoadStats();
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

    public void AddStats(StatType statType, int value)
    {
        foreach (var i in stats)
        {
            if (i.statType == statType)
            {
                i.CurrentValue += value;
            }
        }
        string op = (value > 0) ? "+" : "-";
        OnStatChangeRequested?.Invoke($"{conditionStrDic[(ConditionType)statType]} {op} {value}");
    }
}
