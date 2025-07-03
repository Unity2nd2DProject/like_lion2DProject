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
    private int currentQuestIndex = 0;

    private QuestData currentQuest => quests.Count > currentQuestIndex ? quests[currentQuestIndex] : null;

    protected override void Awake()
    {
        base.Awake();
    }
    private void Start()
    {
        StartQuest(); // Test
    }

    private void StartQuest()
    {
        if (currentQuest != null)
        {
            Debug.Log($"[Quest] ==== 시작! {currentQuest.questName} - {currentQuest.description} ====");
            foreach (var goal in currentQuest.goals)
            {
                goal.currentAmount = 0;
            }
            // UI Update
        }
    }

    public void ReportAction(QuestTargetType actionType)
    {
        if (currentQuest == null)
        {
            return;
        }

        bool anyUpdated = false;

        foreach (var goal in currentQuest.goals)
        {
            if (goal.targetType == actionType && !goal.IsComplete)
            {
                goal.Report();
                anyUpdated = true;

                Debug.Log($"[Quest] {actionType} 진행......  {goal.currentAmount}/{goal.requiredAmount}");
            }
        }

        if (anyUpdated && IsQuestComplete())
        {
            CompleteQuest();
        }
    }

    private bool IsQuestComplete()
    {
        return currentQuest.goals.TrueForAll(g => g.IsComplete);
    }


    private void CompleteQuest()
    {
        Debug.Log($"[Quest] 퀘스트 완료! {currentQuest.questName}");

        if (currentQuest.rewardItem != null)
        {
            InventoryManager.Instance.AddItem(currentQuest.rewardItem, currentQuest.rewardQuantity);
        }

        currentQuestIndex++;
        StartQuest();
    }
}
