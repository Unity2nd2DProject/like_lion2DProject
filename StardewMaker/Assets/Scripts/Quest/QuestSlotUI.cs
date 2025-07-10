using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestSlotUI : MonoBehaviour
{
    private QuestInstance questInstance;
    private QuestData questData;

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI questNameText;
    [SerializeField] private TextMeshProUGUI isCompletedText;
    [SerializeField] private Button button;

    private void Awake()
    {
        if (button != null)
        {
            button.onClick.AddListener(OnClick);
        }
    }

    public void Setup(string questName, bool isCompleted, QuestInstance instance = null, QuestData data = null)
    {
        questInstance = instance;
        questData = data;

        questNameText.text = questName;
        isCompletedText.text = isCompleted ? "(완료)" : "(진행중)";
    }

    public void OnClick()
    {
        if (questInstance != null)
        {
            QuestUI.Instance.ShowQuestDetail(questInstance);
        }
        else if (questData != null)
        {
            QuestUI.Instance.ShowQuestDetail(questData);
        }
    }
}
