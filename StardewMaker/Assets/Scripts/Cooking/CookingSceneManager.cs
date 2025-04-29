using System;
using UnityEngine;
using UnityEngine.UI;

public class CookingSceneManager : MonoBehaviour
{
    public GameObject invenTogleButton;
    public GameObject cookingTogleButton;

    public GameObject cookingUI;
    public GameObject inventoryAndQuickSlotUI;


    private void Awake()
    {
        cookingUI.SetActive(false);
        Initialize();
    }

    private void Initialize()
    {
        cookingTogleButton.GetComponent<Button>().onClick.AddListener(ToggleCookingUI);
        invenTogleButton.GetComponent<Button>().onClick.AddListener(ToggleInventoryUI);
    }

    private void ToggleCookingUI()
    {
        cookingUI.SetActive(!cookingUI.activeSelf);
    }

    private void ToggleInventoryUI()
    {
        inventoryAndQuickSlotUI.SetActive(!inventoryAndQuickSlotUI.activeSelf);
    }
}
