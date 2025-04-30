using UnityEngine;
using UnityEngine.UI;

public class TimeImageUI : Singleton<TimeImageUI>
{
    //public static TimeImageUI Instance;

    public Image timeImage;
    public Sprite daySprite;
    public Sprite nightSprite;

    protected override void Awake()
    {
        base.Awake();
    }

    public void SetDayImage()
    {
        timeImage.sprite = daySprite;
    }

    public void SetNightImage()
    {
        timeImage.sprite = nightSprite;
    }
}
