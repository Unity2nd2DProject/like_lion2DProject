using Unity.VisualScripting;
using UnityEngine;

public class ShopTrigger : MonoBehaviour
{
    [Header("UI References")]
    public GameObject dialogUI;

    [Header("Interaction")]
    [SerializeField] private float interactionRange = 2.5f;

    private bool isPlayerNearby = false;

    private void OnMouseDown()
    {
        if (!CheckDistance())
        {
            return;
        }

        dialogUI.SetActive(true);
        UIManager.Instance.HidePopupImmediately();
        SoundManager.Instance.PlaySfxDialog();
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

    private bool CheckDistance()
    {
        Transform playerTransform = PlayerController.Instance.transform;
        float distance = Vector2.Distance(transform.position, playerTransform.position);

        if (distance > interactionRange)
        {
            return false;
        }
        else
        {
            return true;
        }
    }
}