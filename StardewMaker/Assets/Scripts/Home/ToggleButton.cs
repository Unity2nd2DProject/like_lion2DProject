using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ToggleButton : MonoBehaviour
{
    private string TAG = "[ToggleButton]";
    [SerializeField] private TMP_Text text;
    public static event Action<string, bool> OnToggleChangeRequested;

    public Toggle toggle;
    public ScheduleType scheduleType;

    public Graphic targetGraphic;
    private Color onColor; // = Color.gray;
    private Color offColor; // = Color.white;

    void Awake()
    {
        toggle = GetComponent<Toggle>();
        toggle.onValueChanged.AddListener(OnToggleChanged);

        ColorUtility.TryParseHtmlString("#f49946", out onColor);
        ColorUtility.TryParseHtmlString("#ffffff", out offColor);

        UpdateColor(toggle.isOn);
    }

    void OnToggleChanged(bool isOn)
    {
        UpdateColor(isOn);
        OnToggleChangeRequested?.Invoke(GetComponentInChildren<TMP_Text>().text, isOn);
    }

    void UpdateColor(bool isOn)
    {
        if (targetGraphic != null)
        {
            targetGraphic.color = isOn ? onColor : offColor;
        }
    }

    public void SetText(string t)
    {
        text.text = t;
    }

    public void SetScheduleType(ScheduleType st)
    {
        scheduleType = st;
    }

}
