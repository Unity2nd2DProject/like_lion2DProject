using System;
using UnityEngine;
using System.Collections.Generic;


public class ShopUI : MonoBehaviour
{
    public Transform shopItemParent;

    public GameObject shopSlotUIPrefab;

    public GameObject itemInfoUIPrefab;
    [HideInInspector] public GameObject itemInfoUI;

    public GameObject sellPopupUIPrefab;

    GameObject sellPopupUIInstance;


    public void SetShopUI(List<ItemData> itemDatas)
    {
        foreach (Transform child in shopItemParent)
        {
            Destroy(child.gameObject);
        }

        foreach (ItemData itemData in itemDatas)
        {
            GameObject itemSlotObj = Instantiate(shopSlotUIPrefab, shopItemParent);
            ShopSlotUI shopSlotUI = itemSlotObj.GetComponent<ShopSlotUI>();
            shopSlotUI.Setup(itemData, this);
        }
    }

    public void OpenShopUI()
    {
        gameObject.SetActive(true);
    }

    public void CloseShopUI()
    {
        gameObject.SetActive(false);
        Destroy(sellPopupUIInstance);
    }

    public void ShowSellPopUp(ItemSlot itemSlot)
    {
        sellPopupUIInstance = Instantiate(sellPopupUIPrefab, transform);
        sellPopupUIInstance.GetComponent<SellPopupUI>().SetItemSlot(itemSlot);
    }

    public void ShowItemInfoUI(ItemData itemData, Vector2 position)
    {
        
    }
    public void HideItemInfoUI()
    {
        
    }
}