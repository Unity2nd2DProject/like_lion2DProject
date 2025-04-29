using UnityEngine;
using UnityEngine.UI;

public class TimeImageUI : MonoBehaviour
{
    public static TimeImageUI Instance;

    private Image timeImage;
    public Sprite daySprite;
    public Sprite nightSprite;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        timeImage = GetComponent<Image>();

        //SetDayImage();
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
