using UnityEngine;

public class PopupTrigger : MonoBehaviour
{
    private bool isPlayerNearby = false;

    public string popupText;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = true;
            UIManager.Instance.ShowPopup(popupText, new Vector3(Screen.width / 2f, Screen.height / 1.2f));
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
