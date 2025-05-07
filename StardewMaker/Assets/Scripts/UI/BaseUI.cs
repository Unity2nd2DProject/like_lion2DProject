using TMPro;
using UnityEngine;

public class BaseUI : Singleton<BaseUI>
{        
    public TextMeshProUGUI dateText;
    public TextMeshProUGUI timeText;
    public TextMeshProUGUI moneyText;

    protected override void Awake()
    {
        base.Awake();
    }

    public void SetDateText(string text)
    {
        dateText.text = text;
    }

    public void SetTimeText(string text)
    {
        timeText.text = text;
    }

    public void SetMoneyText(string text)
    {
        moneyText.text = text;
    }

}
