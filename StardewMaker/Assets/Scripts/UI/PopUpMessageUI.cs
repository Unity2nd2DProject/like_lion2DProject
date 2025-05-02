using TMPro;
using UnityEngine;

public class PopUpMessageUI : MonoBehaviour
{
    public TextMeshProUGUI popUpMessage;
    private void Start()
    {
        Destroy(gameObject, 2f); // Destroy the popup after 2 seconds
    }

    public void SetMessage(string message)
    {
        popUpMessage.text = message;
    }
}
