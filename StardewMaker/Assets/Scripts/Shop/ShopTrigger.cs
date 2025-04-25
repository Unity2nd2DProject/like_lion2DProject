using UnityEngine;

public class ShopTrigger : MonoBehaviour
{
    [Header("UI References")]
    public GameObject shopUI;   // ShopUI 루트 오브젝트
    public GameObject inventoryUI;  // InventoryUI 루트 오브젝트

    private bool isPlayerNearby = false;

    void Update()
    {
        HandleShopToggleInput();
        HandleEscapeCloseInput();
    }

    // Enter 키로 상점/인벤토리 UI를 토글
    private void HandleShopToggleInput()
    {
        if (!isPlayerNearby) return;

        if (Input.GetKeyDown(KeyCode.Return))
        {
            bool isActive = !shopUI.activeSelf;

            shopUI.SetActive(isActive);
            inventoryUI.SetActive(isActive);

            Time.timeScale = isActive ? 0f : 1f; // UI 열릴 때 게임 일시 정지
        }
    }

    // ESC 키로 상점 닫기
    private void HandleEscapeCloseInput()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (ShopUI.Instance != null && ShopUI.Instance.gameObject.activeSelf)
            {
                ShopUI.Instance.Close();
                inventoryUI.SetActive(false);
                Time.timeScale = 1f;
            }
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