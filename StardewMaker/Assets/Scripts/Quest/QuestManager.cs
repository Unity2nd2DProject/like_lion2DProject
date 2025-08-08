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
    [SerializeField] private List<QuestData> tutorials;
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
        AcceptQuest(tutorials[0].questID);
    }

    public void AcceptQuest(string questID, string giverNpcName = null)
    {
        if (completedQuestIDs.Contains(questID))
        {
            //Debug.Log($"[Quest] {questID} 수락 실패 1");
            return;
        }
        if (activeQuests.Exists(q => q.questData.questID == questID))
        {
            //Debug.Log($"[Quest] {questID} 수락 실패 2");
            return;
        }

        QuestData questToAccept = quests.Find(q => q.questID == questID);
        if (questToAccept != null)
        {
            QuestInstance instance = new QuestInstance(questToAccept, giverNpcName);
            activeQuests.Add(instance);
            Debug.Log($"========== [Quest] {questToAccept.questName} 시작! ==========");
            UpdateItemCollectGoals();
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
                if (quest.questData.questType == QuestType.Tutorial)
                {
                    CompleteQuest(quest);
                    completed.Add(quest);
                }
                else
                {
                    Debug.Log($"[Quest] {quest.questData.questName} 조건 완료, NPC와 대화를 하세요");
                }
            }
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
                if (quest.questData.questType == QuestType.Tutorial)
                {
                    CompleteQuest(quest);
                    completed.Add(quest);
                }
                else
                {
                    Debug.Log($"[Quest] {quest.questData.questName} 조건 완료, NPC와 대화를 하세요");
                }
            }
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


    public void CompleteQuest(QuestInstance quest)
    {
        Debug.Log($"[Quest] {quest.questData.questName} 완료!");

        foreach (var goal in quest.goals)
        {
            if (goal.goalType == QuestGoalType.ItemCollect && goal.targetItem != null)
            {
                bool success = InventoryManager.Instance.RemoveItem(goal.targetItem, goal.requiredAmount);
                if (!success)
                {
                    Debug.LogWarning($"[Quest] {goal.targetItem.itemName} 삭제 실패! 수량 부족");
                }
            }
        }

        var q = quest.questData;
        if (q.rewardItem != null)
        {
            InventoryManager.Instance.AddItem(q.rewardItem, q.rewardQuantity);
        }
        if (q.rewardMoney > 0)
        {
            InventoryManager.Instance.PlayerMoney += q.rewardMoney;
        }
        if (q.friendshipPointReward > 0)
        {
            FriendshipManager.Instance.AddPoints(q.npcName, q.friendshipPointReward);
        }

        activeQuests.Remove(quest);
        completedQuestIDs.Add(quest.questData.questID);

        if (q.questType == QuestType.Tutorial)
        {
            int currentIndex = tutorials.FindIndex(t => t.questID == q.questID);
            int nextIndex = currentIndex + 1;

            if (nextIndex < tutorials.Count)
            {
                AcceptQuest(tutorials[nextIndex].questID);
            }
            else
            {
                Debug.Log("[Quest] 튜토리얼 모두 완료!");
            }
        }
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

    public void NextDay()
    {
        List<QuestInstance> activeToRemove = new();
        List<string> completedToRemove = new();

        foreach (var questInstance in activeQuests)
        {
            if (questInstance.questData.questType == QuestType.DailyQuest)
            {
                activeToRemove.Add(questInstance);
            }
        }

        foreach (var questID in completedQuestIDs)
        {
            var quest = quests.Find(q => q.questID == questID);
            if (quest != null && quest.questType == QuestType.DailyQuest)
            {
                completedToRemove.Add(questID);
            }
        }

        foreach (var quest in activeToRemove)
        {
            activeQuests.Remove(quest);
        }

        foreach (var id in completedToRemove)
        {
            completedQuestIDs.Remove(id);
        }
    }
}