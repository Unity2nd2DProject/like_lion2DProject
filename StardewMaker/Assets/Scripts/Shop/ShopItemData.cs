using UnityEngine;

[CreateAssetMenu(menuName = "Shop/ShopItemData")]

public class ShopItemData : ScriptableObject
{
    //굳이 ShopItemData으로 나누지 않고, 영현님 작업하신 ItemData랑 합칠 수 있음
    public ItemData itemData;
    public int buyPrice;
    public int sellPrice;
    public string itemDescription;
}