using UnityEngine;
using UnityEngine.UI;

public class QuestUI : Singleton<QuestUI>
{
    [Header("UI")]
    [SerializeField] private GameObject questPanel;
    // [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private Transform ongoingListParent;
    [SerializeField] private Transform completedListParent;
    [SerializeField] private GameObject questSlotPrefab;
    // [SerializeField] private RectTransform scrollRectTransform;
    [SerializeField] private QuestDetailPopupUI questDetailPopupUI;

    // [Header("Scroll")]
    // [SerializeField] private RectTransform scrollViewport;

    protected override void Awake()
    {
        base.Awake();
        ToggleQuestPanel();
    }

    public void ToggleQuestPanel()
    {
        bool isActive = questPanel.activeSelf;
        questPanel.SetActive(!isActive);

        if (!isActive)
        {
            RefreshQuestList();
        }
    }

    public void RefreshQuestList()
    {
        foreach (Transform child in ongoingListParent)
        {
            Destroy(child.gameObject);
        }
        foreach (Transform child in completedListParent)
        {
            Destroy(child.gameObject);
        }

        foreach (var quest in QuestManager.Instance.ActiveQuests)
        {
            var go = Instantiate(questSlotPrefab, ongoingListParent);
            var ui = go.GetComponent<QuestSlotUI>();
            if (ui == null)
            {
                continue;
            }
            ui.Setup(quest.questData.questName, false, quest, quest.questData);
        }

        foreach (var quest in QuestManager.Instance.CompletedQuestDatas)
        {
            var go = Instantiate(questSlotPrefab, completedListParent);
            var ui = go.GetComponent<QuestSlotUI>();
            if (ui == null)
            {
                continue;
            }
            ui.Setup(quest.questName, true, null, quest);
        }
    }

    public void ShowQuestDetail(QuestInstance quest)
    {
        questDetailPopupUI.Show(quest);
    }

    public void ShowQuestDetail(QuestData quest)
    {
        questDetailPopupUI.Show(quest);
    }
}
