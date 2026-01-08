using System.Collections.Generic;
using UnityEngine;


public class QuestManager : Singleton<QuestManager>
{
    [Header("Quests")]
    [SerializeField] private List<QuestDataSO> tutorials;
    [SerializeField] private List<QuestDataSO> quests;

    [Header("진행중인 퀘스트 (테스트 용)")]
    [SerializeField] private List<QuestInstance> activeQuests = new();
    [SerializeField] private HashSet<string> completedQuestIDs = new();

    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        SaveLoadManager.Instance.LoadQuest();
        if (!HasCompletedQuest(tutorials[0].questID) && !IsQuestActive(tutorials[0].questID))
        {
            AcceptQuest(tutorials[0].questID);
        }
    }

    public void AcceptQuest(string questID, NPC.NpcId giverNpcName = NPC.NpcId.None)
    {
        if (completedQuestIDs.Contains(questID))
        {
            Debug.Log($"[Quest] {questID} 수락 실패 [이미 완료한 퀘스트]");
            return;
        }
        if (activeQuests.Exists(q => q.questData.questID == questID))
        {
            Debug.Log($"[Quest] {questID} 수락 실패 [현재 진행중인 퀘스트]");
            return;
        }

        QuestDataSO questToAccept = quests.Find(q => q.questID == questID);
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
        UIManager.Instance.ShowPopup($"\"{quest.questData.questName}\" 완료!", new Vector3(Screen.width / 2f, Screen.height / 1.2f));

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
        foreach (var reward in q.rewards)
        {
            if(reward.rewardType == RewardType.Item)
            {
                InventoryManager.Instance.AddItem(reward.item, reward.itemQuantity);
                Debug.Log($"[Quest] 보상: {reward.item.itemName} x{reward.itemQuantity} 획득");
            }
            else if (reward.rewardType == RewardType.Money)
            {
                InventoryManager.Instance.PlayerMoney += reward.money;
                Debug.Log($"[Quest] 보상: {reward.money} 골드 획득");
            }
            else if (reward.rewardType == RewardType.FriendshipPoint)
            {
                FriendshipManager.Instance.AddPoints(reward.npc, reward.friendshipPoint);
                Debug.Log($"[Quest] 보상: {reward.npc}에게 우정 포인트 {reward.friendshipPoint} 획득");
            }
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

    public List<QuestDataSO> CompletedQuestDatas
    {
        get
        {
            List<QuestDataSO> list = new();
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

    public SavedQuestData SaveQuests()
    {
        SavedQuestData data = new SavedQuestData();

        foreach (var quest in activeQuests)
        {
            SavedQuest sq = new SavedQuest
            {
                questID = quest.questData.questID,
                giverNpcId = quest.giverNpcId
            };

            foreach (var goal in quest.goals)
            {
                sq.currentAmounts.Add(goal.currentAmount);
            }

            data.activeQuests.Add(sq);
        }

        data.completedQuestIDs.AddRange(completedQuestIDs);
        return data;
    }

    public void LoadQuests(SavedQuestData data)
    {
        activeQuests.Clear();
        completedQuestIDs.Clear();

        foreach (var saved in data.activeQuests)
        {
            QuestDataSO questData = quests.Find(q => q.questID == saved.questID);
            if (questData == null)
            {
                Debug.LogWarning($"[QuestLoad] {saved.questID} 퀘스트 데이터를 찾을 수 없습니다.");
                continue;
            }

            QuestInstance instance = new QuestInstance(questData, saved.giverNpcId);

            for (int i = 0; i < saved.currentAmounts.Count && i < instance.goals.Count; i++)
            {
                instance.goals[i].currentAmount = saved.currentAmounts[i];
            }

            activeQuests.Add(instance);
        }

        foreach (var id in data.completedQuestIDs)
        {
            completedQuestIDs.Add(id);
        }
    }

}