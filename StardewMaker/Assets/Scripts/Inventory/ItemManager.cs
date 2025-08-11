using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : Singleton<ItemManager>
{
    public List<ItemData> items;

    public ItemData GetItemByName(string itemName)
    {
        return items.Find(item => item.itemName == itemName);
    }
}
