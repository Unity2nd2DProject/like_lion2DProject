using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GiftInfoUI : MonoBehaviour
{
    public Image giftIcon; // 선물 아이콘
    public TextMeshProUGUI giftName; // 선물 이름
    public TextMeshProUGUI giftDescription; // 선물 설명

    public GameObject giftButton; // 선물 버튼

    private ItemData currentGiftItem; // 현재 선물 아이템   

    private void Start()
    {
        // 선물 버튼 클릭 시 선물 정보 UI 활성화
        giftButton.GetComponent<Button>().onClick.AddListener(OnGiftButtonClicked);
    }
    private void OnGiftButtonClicked()
    {
        // 선물 아이콘, 이름, 설명 업데이트
        GiftManager.Instance.Gift(currentGiftItem);
    }

    public void UpdateGiftInfo(ItemData giftItem)
    {
        currentGiftItem = giftItem; // 현재 선물 아이템 저장

        giftIcon.sprite = giftItem.icon; // 선물 아이콘 업데이트
        giftName.text = giftItem.itemName; // 선물 이름 업데이트
        giftDescription.text = giftItem.itemDescription; // 선물 설명 업데이트
    }
}
