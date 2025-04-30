using UnityEngine;
using UnityEngine.UI;

public class BackgroundController : MonoBehaviour
{
    [SerializeField] private GameObject background;
    [SerializeField] private Sprite daySprite;
    [SerializeField] private Sprite nightSprite;

    public void SetDaySprite()
    {
        background.GetComponent<SpriteRenderer>().sprite = daySprite;
    }

    public void SetNightSprite()
    {
        background.GetComponent<SpriteRenderer>().sprite = nightSprite;
    }
}
