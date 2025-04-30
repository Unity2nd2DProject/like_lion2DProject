using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class QuickSlotUI : MonoBehaviour
{
    InventoryManager inventoryManager;
    public List<SlotUI> quickSlotSlotUIs = new List<SlotUI>();
    public GameObject currentSelectedCursor;

    private void Awake()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if(GameManager.Instance.currentMode == GameMode.TOWN)
        {
            ShowQuickSlot();
        }
        else if (GameManager.Instance.currentMode == GameMode.HOME)
        {
            HideQuickSlot();
        }
    }




    private void ShowQuickSlot()
    {
        this.gameObject.SetActive(true);
    }

    private void HideQuickSlot()
    {
        this.gameObject.SetActive(false);
    }

    public void InitializeQuickSlotUI()
    {
        inventoryManager = InventoryManager.Instance;
        UpdateQuickSlotUI();
    }

    public void UpdateQuickSlotUI()
    {
        for (int i = 0; i < inventoryManager.quickSlotSize; i++)
        {
            quickSlotSlotUIs[i].UpdateSlot(inventoryManager.slots[inventoryManager.inventorySize + i]);
        }
        UpdateSelectedSlot();
    }

    private void UpdateSelectedSlot()
    {
        currentSelectedCursor.transform.SetParent(quickSlotSlotUIs[inventoryManager.currentSelectedQuickSlotIndex].transform);
        currentSelectedCursor.transform.localPosition = new Vector3(0, 0, 0);
    }
}
