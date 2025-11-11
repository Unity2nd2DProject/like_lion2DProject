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
    
    public void ShowSellPopUp(ItemSlot itemSlot)
    {
        Instantiate(sellPopupUIPrefab, transform.parent);
        SellPopupUI sellPopupUI = sellPopupUIPrefab.GetComponent<SellPopupUI>();
        Debug.Log(itemSlot);
        sellPopupUI.SetItemSlot(itemSlot);
    }

    public void ShowItemInfoUI(ItemData itemData, Vector2 position)
    {
        
    }
    public void HideItemInfoUI()
    {
        
    }
}