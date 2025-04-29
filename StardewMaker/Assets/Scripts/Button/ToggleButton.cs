using UnityEngine;
using UnityEngine.UI;

public class ToggleButton : MonoBehaviour
{
    private string TAG = "[ToggleButton]";
    private Toggle toggle;

    public Graphic targetGraphic;
    public Color onColor = Color.gray;
    public Color offColor = Color.white;
    void Awake()
    {
        toggle = GetComponent<Toggle>();
        toggle.onValueChanged.AddListener(OnToggleChanged);

        UpdateColor(toggle.isOn);
    }

    void OnToggleChanged(bool isOn)
    {
        UpdateColor(isOn);
    }

    void UpdateColor(bool isOn)
    {
        if (targetGraphic != null)
        {
            targetGraphic.color = isOn ? onColor : offColor;
        }
    }
}
