using System;
using UnityEngine;

public class GiftManager : Singleton<GiftManager>
{

    protected override void Awake()
    {
        base.Awake();

    }
    private void Start()
    {
        // UIManager.Instance.InitializeGiftUI();
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
            //Debug.Log("Gift received!");
            InventoryManager.Instance.RemoveItem(giftItem);

            if(giftItem.itemType == ItemType.Food)
            {
                // 선물 아이템이 Food일 경우
                DaughterManager.Instance.AddStats(StatType.HUNGER, 5);
                QuestManager.Instance.ReportAction(QuestTargetType.CookedFood);
            }
            else
            {
                Array values = Enum.GetValues(typeof(StatType));                
                StatType randomStat = (StatType)values.GetValue(UnityEngine.Random.Range(5, 10));
                DaughterManager.Instance.AddStats(randomStat,1);

            }
            QuestManager.Instance.ReportAction(QuestTargetType.GaveToDaughter);
            // UIManager.Instance.ToggleGiftUI();
        }            
    }
}
