using System;
using UnityEngine;
using UnityEngine.UI;

public class RightButtonUI : MonoBehaviour
{
    public Button questButton;
    public Button inventoryButton;
    public Button optionButton;

    private void Start()
    {
        if (questButton)
        {
            questButton.onClick.RemoveAllListeners();
        }
        if (inventoryButton)
        {
            inventoryButton.onClick.RemoveAllListeners();
        }
        if (optionButton)
        {
            optionButton.onClick.RemoveAllListeners();
        }
    }

    private void OnQuestButtonClicked()
    {
        UIManager.Instance.ToggleQuestPanel();
    }

    private void OnOptionButtonClicked()
    {
        UIManager.Instance.ToggleSoundSettingUI();
    }

    private void OnInventoryButtonClicked()
    {
        // UIManager.Instance.ToggleInventoryByButton();
    }
}
