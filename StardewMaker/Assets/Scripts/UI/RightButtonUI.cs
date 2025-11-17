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
        questButton.onClick.AddListener(OnQuestButtonClicked);
        inventoryButton.onClick.AddListener(OnInventoryButtonClicked);
        optionButton.onClick.AddListener(OnOptionButtonClicked);
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
        UIManager.Instance.ToggleInventoryByButton();
    }
}
