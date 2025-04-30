using UnityEngine;
using UnityEngine.UI;

public class TimeImageUI : Singleton<TimeImageUI>
{
    //public static TimeImageUI Instance;

    private Image timeImage;
    public Sprite daySprite;
    public Sprite nightSprite;

    protected override void Awake()
    {
        base.Awake();

        timeImage = GetComponent<Image>();
    }

    //private void Awake()
    //{
    //    if (Instance == null)
    //    {
    //        Instance = this;
    //    }

    //    timeImage = GetComponent<Image>();
    //}

    public void SetDayImage()
    {
        timeImage.sprite = daySprite;
    }

    public void SetNightImage()
    {
        timeImage.sprite = nightSprite;
    }
}
