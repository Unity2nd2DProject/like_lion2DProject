using System.Collections.Generic;
using UnityEngine;

public class StatUI : MonoBehaviour
{
    [SerializeField] private StatRowUI moodRow;
    [SerializeField] private StatRowUI vitalityRow;
    [SerializeField] private StatRowUI hungerRow;
    [SerializeField] private StatRowUI trustRow;

    private Dictionary<StatType, StatRowUI> rowDict;

    private void Awake()
    {
        rowDict = new Dictionary<StatType, StatRowUI>
        {
            { StatType.MOOD, moodRow },
            { StatType.VITALITY, vitalityRow },
            { StatType.HUNGER, hungerRow },
            { StatType.TRUST, trustRow }
        };
    }

    public void Initialize(List<Stat> stats)
    {
        foreach (var stat in stats)
        {
            if (rowDict.TryGetValue(stat.statType, out var row))
            {
                row.Bind(stat);
            }
        }
    }
}
