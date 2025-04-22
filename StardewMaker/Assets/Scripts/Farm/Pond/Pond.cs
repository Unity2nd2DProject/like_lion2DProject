using UnityEngine;

public class Pond : MonoBehaviour
{
    public ItemData waterData;

    public void GetWater()
    {
        Inventory.Instance.AddItem(waterData, 1);
    }
}
