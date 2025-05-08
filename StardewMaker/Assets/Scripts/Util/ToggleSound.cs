using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Toggle))]
public class ToggleSound : MonoBehaviour
{
    private void Awake()
    {
        Toggle toggle = GetComponent<Toggle>();
        toggle.onValueChanged.AddListener(OnToggleChanged);
    }

    private void OnToggleChanged(bool isOn)
    {
        if (isOn)
        {
            SoundManager.Instance.PlaySFX("Click");
        }
        else
        {
            SoundManager.Instance.PlaySFX("Cancel");
        }
    }
}
