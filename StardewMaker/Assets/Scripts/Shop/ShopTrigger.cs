using UnityEngine;

public class ShopTrigger : MonoBehaviour
{
    public GameObject shopUI;
    public GameObject sellUI;
    private bool isPlayerNearby;

    void Update()
    {
        if (isPlayerNearby && Input.GetKeyDown(KeyCode.Return))
        {
            shopUI.SetActive(!shopUI.activeSelf);
            sellUI.SetActive(!sellUI.activeSelf);

            Time.timeScale = shopUI.activeSelf ? 0f : 1f;
            Time.timeScale = sellUI.activeSelf ? 0f : 1f;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = false;
        }
    }
}
