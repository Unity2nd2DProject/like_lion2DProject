using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : Singleton<ItemManager> // 해당 클래스는 불 필요. 모든 아이템의 정보가 필요한 경우가 존재하지 않는다고 생각.
{
    public List<ItemData> items;

    public ItemData GetItemByName(string itemName)
    {
        return items.Find(item => item.itemName == itemName);
    }
}
