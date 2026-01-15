using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestSlotUI : MonoBehaviour
{
    private QuestInstance questInstance;
    private QuestDataSO questData;

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

    public void Setup(string questName, bool isCompleted, QuestInstance instance = null, QuestDataSO data = null)
    {
        questInstance = instance;
        questData = data;

        questNameText.text = questName;
        isCompletedText.text = isCompleted ? "(완료)" : "(진행중)";
    }

    public void OnClick()
    {
        // UIManager.Instance.questUIController.questDetailPopupUI.Setup(questInstance, questData);
    }
}
