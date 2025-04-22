using UnityEngine;
using UnityEngine.UI;

public class CurrentToolUI : MonoBehaviour
{
    public static CurrentToolUI Instance;
    public Image icon;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        //icon = GetComponent<Image>();
    }

    public void SetIcon(Sprite _icon)
    {
        icon.sprite = _icon;
    }
}
