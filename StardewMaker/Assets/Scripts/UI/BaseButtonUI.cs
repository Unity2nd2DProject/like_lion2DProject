using System;
using UnityEngine;
using UnityEngine.UI;

public class BaseButtonUI : MonoBehaviour
{
    public Button inventoryButton;
    public Button optionButton;

    private void Start()
    {
        inventoryButton.onClick.AddListener(OnInventoryButtonClicked);
        optionButton.onClick.AddListener(OnOptionButtonClicked);
    }

    private void OnOptionButtonClicked()
    {
        UIManager.Instance.ToggleSoundSettingUI();
    }

    private void OnInventoryButtonClicked()
    {
        UIManager.Instance.ToggleInventoryByButton();
    }
}
