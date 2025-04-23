using UnityEngine;

public class Tree : MonoBehaviour
{
    private int maxHits = 3;
    private int currentHits = 0;
    public ItemData woodData;

    public void Chop()
    {
        currentHits++;
        Inventory.Instance.AddItem(woodData, 1);

        if (currentHits >= maxHits)
        {
            gameObject.SetActive(false);
        }
    }

    public void NexDay()
    {
        currentHits = 0;
        gameObject.SetActive(true);
    }
}
