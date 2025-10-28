using UnityEngine;
using UnityEngine.UI;

public class TimeImageUI : Singleton<TimeImageUI>
{

    public Image timeImage;
    public Sprite daySprite;
    public Sprite nightSprite;

    public void SetDayImage()
    {
        timeImage.sprite = daySprite;
    }

    public void SetNightImage()
    {
        timeImage.sprite = nightSprite;
    }
}
