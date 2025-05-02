using TMPro;
using UnityEngine;

public class StatRowUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI statText;
    [SerializeField] private Stat stat;

    bool isEndingStat;

    public void Bind(Stat stat, bool isEndingStat)
    {
        this.stat = stat;
        this.isEndingStat = isEndingStat;
        
        if (!isEndingStat)
        {            
            statText.text = $"{stat.statType.ToKorean()}: {stat.CurrentValue}/{stat.MaxValue}";
            stat.OnValueChanged += UpdateUI;
        }
        else
        {
            statText.text = $"{stat.statType.ToKorean()}: {stat.CurrentValue}";
            stat.OnValueChanged += UpdateUI;
        }
        
    }
    private void UpdateUI(float current)
    {
        if (!isEndingStat)
        {
            statText.text = $"{stat.statType.ToKorean()}: {current}/{stat.MaxValue}";
        }
        else
        {
            statText.text = $"{stat.statType.ToKorean()}: {current}";
        }
    }
}
