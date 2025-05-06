using UnityEngine;
using UnityEngine.UI;

public class GiftUI : MonoBehaviour
{
    public Button quitButton;

    public GameObject GiftInfoUI; // 선물 정보 UI
    public GameObject GiftInventoryUI; // 선물 인벤토리 UI

    void CloseUI()
    {
        UIManager.Instance.CloseGiftUI();
    }

    private void Start()
    {
        quitButton.onClick.AddListener(CloseUI);
        // quitButton.onClick.AddListener(OnQuickButtonClicked);
        GiftInventoryUI.GetComponent<GiftInventoryUI>().InitializeGiftInventoryUI(); // 선물 인벤토리 UI 초기화
    }

    // private void OnQuickButtonClicked()
    // {
    //     gameObject.SetActive(false); // UI 비활성화
    // }

    public void SetGift(ItemData gift)
    {
        GiftInfoUI.GetComponent<GiftInfoUI>().UpdateGiftInfo(gift); // 선물 정보 UI 업데이트
    }

}
