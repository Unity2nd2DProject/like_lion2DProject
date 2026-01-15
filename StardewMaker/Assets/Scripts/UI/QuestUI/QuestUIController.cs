using UnityEngine;
using UnityEngine.UI;

public class QuestUIController : MonoBehaviour { 

    [Header("UI")]
    [SerializeField] private GameObject questPanel;
    [SerializeField] private Transform ongoingListParent;
    [SerializeField] private Transform completedListParent;
    [SerializeField] private GameObject questSlotPrefab;
    [SerializeField] private QuestDetailPopupUI questDetailPopupUI;
    [SerializeField] private Button toggleButton;


    private void Start()
    {
        toggleButton.onClick.AddListener(ToggleQuestPanel);

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

        Debug.Log("퀘스트 목록 새로고침");
    }

    public void ShowQuestDetail(QuestInstance quest)
    {
        questDetailPopupUI.Show(quest);
    }

    public void ShowQuestDetail(QuestDataSO quest)
    {
        questDetailPopupUI.Show(quest);
    }

    public void CloseQuestUI()
    {
        questPanel.SetActive(false);
    }
}
