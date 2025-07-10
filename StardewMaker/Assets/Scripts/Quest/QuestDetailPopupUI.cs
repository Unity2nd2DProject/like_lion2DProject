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
        foreach (var goal in quest.goals)
        {
            goals += $"- {goal.targetType}: {goal.currentAmount}/{goal.requiredAmount}\n";
        }
        goalText.text = goals;
    }

    public void Show(QuestData questData)
    {
        popupPanel.SetActive(true);
        nameText.text = questData.questName;
        descriptionText.text = questData.description;

        string goals = "";
        foreach (var goal in questData.goals)
        {
            goals += $"- {goal.targetType}: {goal.requiredAmount}\n";
        }
        goalText.text = goals;
    }

    public void Hide()
    {
        popupPanel.SetActive(false);
    }
}
