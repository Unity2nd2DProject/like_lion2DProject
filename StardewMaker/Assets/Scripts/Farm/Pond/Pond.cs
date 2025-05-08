using System.Collections;
using UnityEngine;

public class Pond : MonoBehaviour
{
    public ItemData waterData;
    public ItemData[] Fishes;

    public void GetWater()
    {
        InventoryManager.Instance.AddItem(waterData, 10);
    }

    public void Fish()
    {
        //StartCoroutine(FishingRoutine());

        ItemData caughtFish = Fishes[Random.Range(0, Fishes.Length)];
        Debug.Log($"get {caughtFish.itemName}!");
        UIManager.Instance.ShowPopup($"{caughtFish.itemName}을(를) 잡았다!", new Vector3(Screen.width / 2f, Screen.height / 1.2f));
        InventoryManager.Instance.AddItem(caughtFish, 1);
    }

    //IEnumerator FishingRoutine()
    //{
    //    float waitTime = Random.Range(1f, 3f);
    //    yield return new WaitForSeconds(waitTime);

    //    ItemData caughtFish = Fishes[Random.Range(0, Fishes.Length)];
    //    Debug.Log($"get {caughtFish.itemName}!");
    //    Inventory.Instance.AddItem(caughtFish, 1);
    //}
}
