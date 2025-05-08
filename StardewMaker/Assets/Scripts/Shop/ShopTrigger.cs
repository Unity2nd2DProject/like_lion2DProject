using UnityEngine;

public class ShopTrigger : MonoBehaviour
{
    [Header("UI References")]
    public GameObject dialogUI;

    private bool isPlayerNearby = false;

    void Update()
    {
        ShowDialogUI();
    }

    // Enter 키로 다이얼로그 UI 띄움
    private void ShowDialogUI()
    {
        if (!isPlayerNearby) return;

        if (Input.GetKeyDown(KeyCode.Return))
        {
            dialogUI.SetActive(true);

            SoundManager.Instance.PlaySfxDialog();
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