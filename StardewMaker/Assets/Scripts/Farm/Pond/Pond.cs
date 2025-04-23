using System.Collections;
using UnityEngine;

public class Pond : MonoBehaviour
{
    public ItemData waterData;
    public ItemData[] Fishes;

    public void GetWater()
    {
        Inventory.Instance.AddItem(waterData, 10);
    }

    public void Fish()
    {
        StartCoroutine(FishingRoutine());
    }

    IEnumerator FishingRoutine()
    {
        float waitTime = Random.Range(1f, 3f);
        yield return new WaitForSeconds(waitTime);

        ItemData caughtFish = Fishes[Random.Range(0, Fishes.Length)];
        Debug.Log($"get {caughtFish.itemName}!");
        Inventory.Instance.AddItem(caughtFish, 1);
    }
}
