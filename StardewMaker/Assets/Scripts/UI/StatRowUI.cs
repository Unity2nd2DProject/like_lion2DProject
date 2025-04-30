using TMPro;
using UnityEngine;

public class StatRowUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI statText;
    [SerializeField] private Stat stat;

    public void Bind(Stat stat)
    {
        this.stat = stat;
        statText.text = $"{stat.statType.ToKorean()}: {stat.CurrentValue}/{stat.MaxValue}";

        stat.OnValueChanged += UpdateUI;
    }
    private void UpdateUI(float current)
    {
        statText.text = $"{stat.statType.ToKorean()}: {current}/{stat.MaxValue}";
    }
}
