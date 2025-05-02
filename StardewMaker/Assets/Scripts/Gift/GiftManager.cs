using UnityEngine;

public class GiftManager : Singleton<GiftManager>
{
    protected override void Awake()
    {
        base.Awake();
       
    }
    private void Start()
    {
        UIManager.Instance.InitializeGiftUI();
    }

    public void Gift(ItemData giftItem)
    {
        // 선물 아이템을 주는 로직
        // 예: 인벤토리에 선물 아이템 추가
        Debug.Log("Gift received!");

        // 인벤토리에서 선물 아이템 제거
        InventoryManager.Instance.RemoveItem(giftItem);
        // TODO: 선물 아이템을 주고 난 뒤 다이얼로그 추가

        DaughterManager.Instance.AddStats(StatType.MOOD,1);
        DaughterManager.Instance.AddStats(StatType.ART,1);

        UIManager.Instance.ToggleGiftUI();
    }
}
