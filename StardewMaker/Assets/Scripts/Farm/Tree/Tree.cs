using UnityEngine;

public class Tree : MonoBehaviour
{
    private int maxHits = 3;
    private int currentHits = 0;
    public ItemData woodData;

    public void Chop()
    {
        currentHits++;
        InventoryManager.Instance.AddItem(woodData, 1);

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

    public int GetCurrentHits() => currentHits;

    public void SetState(int hits, bool active)
    {
        currentHits = hits;
        gameObject.SetActive(active);
    }

}
