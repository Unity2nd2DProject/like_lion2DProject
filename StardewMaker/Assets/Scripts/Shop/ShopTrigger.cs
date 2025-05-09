using Unity.VisualScripting;
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
            UIManager.Instance.HidePopupImmediately();

            SoundManager.Instance.PlaySfxDialog();
        }
    }

    public void TryShowPopup()
    {
        if (isPlayerNearby && !dialogUI.activeSelf && UIManager.Instance.currentPopup == null && !ShopUI.Instance.isActiveAndEnabled)
        {
            UIManager.Instance.ShowPopup("상점", new Vector3(Screen.width / 2f, Screen.height / 1.2f));
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = true;

            if (!dialogUI.activeSelf && UIManager.Instance.currentPopup == null)
            {
                UIManager.Instance.ShowPopup("상점", new Vector3(Screen.width / 2f, Screen.height / 1.2f));
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = false;
            
            if (UIManager.Instance == null)
            {
                Debug.LogWarning("UIManager.Instance is null on OnTriggerExit2D");
                return;
            }

            if (UIManager.Instance.currentPopup != null)
            {
                UIManager.Instance.HidePopupImmediately();
            }
        }
    }
}