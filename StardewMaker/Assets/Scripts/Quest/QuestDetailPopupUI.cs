using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestDetailPopupUI : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject popupPanel;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI descriptionText;
    [SerializeField] private TextMeshProUGUI goalText;
    [SerializeField] private Button closeButton;

    private void Awake()
    {
        popupPanel.SetActive(false);
        closeButton.onClick.AddListener(Hide);
    }

    public void Show(QuestInstance quest)
    {
        popupPanel.SetActive(true);
        nameText.text = quest.questData.questName;
        descriptionText.text = quest.questData.description;
        string goals = "";

        if (QuestManager.Instance.HasCompletedQuest(quest.questData.questID))
        {
            goalText.text = goals;
            return;
        }

        foreach (var goal in quest.goals)
        {
            string targetText = "";

            if (goal.goalType == QuestGoalType.Action)
            {
                targetText = $"{GetTarget(goal.targetType)}: {goal.currentAmount}/{goal.requiredAmount}";
            }
            else if (goal.goalType == QuestGoalType.ItemCollect)
            {
                targetText = $"{goal.targetItem.itemName}: {goal.currentAmount}/{goal.requiredAmount}";
            }

            if (goal.IsComplete)
            {
                goals += $"<color=#00AA00>- {targetText}</color>\n"; // RichText
            }
            else
            {
                goals += $"- {targetText}\n";
            }
        }
        goalText.text = goals;
    }

    public void Show(QuestData questData)
    {
        popupPanel.SetActive(true);
        nameText.text = questData.questName;
        descriptionText.text = questData.description;
        string goals = "";

        if (QuestManager.Instance.HasCompletedQuest(questData.questID))
        {
            goalText.text = goals;
            return;
        }

        foreach (var goal in questData.goals)
        {
            string targetText = "";

            if (goal.goalType == QuestGoalType.Action)
            {
                targetText = $"{GetTarget(goal.targetType)}: {goal.currentAmount}/{goal.requiredAmount}";
            }
            else if (goal.goalType == QuestGoalType.ItemCollect)
            {
                targetText = $"{goal.targetItem.itemName}: {goal.currentAmount}/{goal.requiredAmount}";
            }

            if (goal.IsComplete)
            {
                goals += $"<color=#00AA00>- {targetText}</color>\n"; // RichText
            }
            else
            {
                goals += $"- {targetText}\n";
            }
        }
        goalText.text = goals;
    }

    private string GetTarget(QuestTargetType targetType)
    {
        switch (targetType)
        {
            case QuestTargetType.TrilledSoil:
                return "밭 갈기";
            case QuestTargetType.SeedPlanted:
                return "씨앗 심기";
            case QuestTargetType.Watered:
                return "물 주기";
            case QuestTargetType.Fertilized:
                return "비료 주기";
            case QuestTargetType.Harvested:
                return "수확하기";
            case QuestTargetType.TreeChopped:
                return "나무 베기";
            case QuestTargetType.FishCaught:
                return "낚시하기";
            case QuestTargetType.StoneBroken:
                return "돌 캐기";
            case QuestTargetType.GaveToDaughter:
                return "딸에게 선물 주기";
            case QuestTargetType.CookedFood:
                return "요리하기";
            case QuestTargetType.GreetedToNPC:
                return "npc에게 인사하기";
            default:
                return "";
        }
    }

    public void Hide()
    {
        popupPanel.SetActive(false);
    }
}
