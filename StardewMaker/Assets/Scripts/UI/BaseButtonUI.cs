using System;
using UnityEngine;
using UnityEngine.UI;

public class BaseButtonUI : MonoBehaviour
{
    public Button inventoryButton;
    public Button optionButton;

    private void Start()
    {
        Debug.Log("BaseButtonUI Start");
        inventoryButton.onClick.AddListener(OnInventoryButtonClicked);
        optionButton.onClick.AddListener(OnOptionButtonClicked);
    }

    private void OnOptionButtonClicked()
    {
        UIManager.Instance.ToggleSoundSettingUI();
    }

    private void OnInventoryButtonClicked()
    {
        Debug.Log("OnInventoryButtonClicked");
        Debug.Log("GameManager.Instance.currentMode: " + GameManager.Instance.currentMode);
        Debug.Log("UIManager.Instance: " + UIManager.Instance);
        UIManager.Instance.ToggleInventoryByButton();
    }
}
