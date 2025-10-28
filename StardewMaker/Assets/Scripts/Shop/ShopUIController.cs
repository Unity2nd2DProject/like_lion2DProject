using System;
using UnityEngine;
using System.Collections.Generic;


public class ShopUIController : MonoBehaviour
{
    public Transform shopItemParent;

    public GameObject shopSlotUIPrefab;

    public GameObject itemInfoUIPrefab;
    [HideInInspector] public GameObject itemInfoUI;


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

    public void ShowItemInfoUI(ItemData itemData, Vector2 position)
    {
        
    }
    public void HideItemInfoUI()
    {
        
    }
}