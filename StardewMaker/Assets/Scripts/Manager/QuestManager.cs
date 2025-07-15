using System.Collections.Generic;
using UnityEngine;

public enum QuestTargetType
{
    TrilledSoil,
    SeedPlanted,
    Watered,
    Fertilized,
    Harvested,
    TreeChopped,
    FishCaught,
    StoneBroken,
    GaveToDaughter,
    CookedFood,
    GreetedToNPC,
}

public class QuestManager : Singleton<QuestManager>
{
    [Header("Quests")]
    [SerializeField] private List<QuestData> quests;

    [Header("Check")]
    [SerializeField] private List<QuestInstance> activeQuests = new();
    [SerializeField] private HashSet<string> completedQuestIDs = new();

    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        // test
        //foreach (var quest in quests)
        //{
        //    AcceptQuest(quest.questID);
        //}
    }

    public void AcceptQuest(string questID)
    {
        if (completedQuestIDs.Contains(questID))
        {
            return;
        }
        if (activeQuests.Exists(q => q.questData.questID == questID))
        {
            return;
        }

        QuestData questToAccept = quests.Find(q => q.questID == questID);
        if (questToAccept != null)
        {
            QuestInstance instance = new QuestInstance(questToAccept);
            activeQuests.Add(instance);
            Debug.Log($"========== [Quest] {questToAccept.questName} 시작! ==========");
        }
    }

    public void ReportAction(QuestTargetType actionType)
    {
        List<QuestInstance> completed = new();

        foreach (var quest in activeQuests)
        {
            bool updated = false;

            foreach (var goal in quest.goals)
            {
                if (goal.goalType == QuestGoalType.Action &&
                    goal.targetType == actionType && !goal.IsComplete)
                {
                    goal.Report();
                    updated = true;

                    Debug.Log($"[Quest] {quest.questData.questName} - ({actionType} {goal.currentAmount}/{goal.requiredAmount})");
                }
            }

            if (updated && quest.IsComplete)
            {
                CompleteQuest(quest);
                completed.Add(quest);
            }
        }

        foreach (var quest in completed)
        {
            activeQuests.Remove(quest);
        }
    }

    public void UpdateItemCollectGoals()
    {
        List<QuestInstance> completed = new();

        foreach (var quest in activeQuests)
        {
            bool updated = false;

            foreach (var goal in quest.goals)
            {
                if (goal.goalType == QuestGoalType.ItemCollect && !goal.IsComplete)
                {
                    int ownedAmount = GetItemCount(goal.targetItem);
                    int newAmount = Mathf.Min(goal.requiredAmount, ownedAmount);

                    if (goal.currentAmount != newAmount)
                    {
                        goal.currentAmount = newAmount;
                        updated = true;

                        Debug.Log($"[Quest] {quest.questData.questName} - {goal.targetItem.itemName}: {goal.currentAmount}/{goal.requiredAmount}");
                    }
                }
            }

            if (updated && quest.IsComplete)
            {
                CompleteQuest(quest);
                completed.Add(quest);
            }
        }

        foreach (var quest in completed)
        {
            activeQuests.Remove(quest);
        }
    }

    private int GetItemCount(ItemData item)
    {
        int count = 0;
        foreach (var slot in InventoryManager.Instance.slots)
        {
            if (slot.itemData == item)
            {
                count += slot.quantity;
            }
        }
        return count;
    }


    private void CompleteQuest(QuestInstance quest)
    {
        Debug.Log($"[Quest] {quest.questData.questName} 완료!");

        var q = quest.questData;
        if (q.rewardItem != null)
        {
            InventoryManager.Instance.AddItem(q.rewardItem, q.rewardQuantity);
        }
        if (q.rewardMoney > 0)
        {
            // money
            InventoryManager.Instance.PlayerMoney += q.rewardMoney;
        }
        if (q.friendshipPointReward > 0)
        {
            FriendshipManager.Instance.AddPoints(q.npcName, q.friendshipPointReward);
        }

        completedQuestIDs.Add(quest.questData.questID);
    }

    public List<QuestInstance> ActiveQuests => activeQuests;

    public bool IsQuestActive(string questID)
    {
        return activeQuests.Exists(q => q.questData.questID == questID);
    }

    public List<QuestData> CompletedQuestDatas
    {
        get
        {
            List<QuestData> list = new();
            foreach (var id in completedQuestIDs)
            {
                var quest = quests.Find(q => q.questID == id);
                if (quest != null)
                    list.Add(quest);
            }
            return list;
        }
    }

    public bool HasCompletedQuest(string questID)
    {
        return completedQuestIDs.Contains(questID);
    }
}