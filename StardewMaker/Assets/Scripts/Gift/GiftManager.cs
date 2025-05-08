using System;
using UnityEngine;

public class GiftManager : Singleton<GiftManager>
{
    public static event Action<SituationType> OnDialogRequested;

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
        if(giftItem == null)
        {
            UIManager.Instance.ShowPopup("선물 아이템이 없습니다!");
            return;
        }
        else
        {
            Debug.Log("Gift received!");
            InventoryManager.Instance.RemoveItem(giftItem);

            if(giftItem.itemType == ItemType.Food)
            {
                // 선물 아이템이 Food일 경우
                OnDialogRequested?.Invoke(SituationType.FOOD_RECEIVED);
                DaughterManager.Instance.AddStats(StatType.HUNGER, 5);
            }
            else
            {
                OnDialogRequested?.Invoke(SituationType.GIFT_RECEIVED);

                Array values = Enum.GetValues(typeof(StatType));                
                StatType randomStat = (StatType)values.GetValue(UnityEngine.Random.Range(5, 10));
                DaughterManager.Instance.AddStats(randomStat,1);

            }
            UIManager.Instance.ToggleGiftUI();
        }            
    }
}
