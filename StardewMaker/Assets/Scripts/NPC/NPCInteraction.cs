using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class NPCInteraction : MonoBehaviour
{
    private NPCQuestGiver questGiver;
    private NPCController npcController;

    [Header("Dialogue")]
    [SerializeField] private string npcName;
    [SerializeField] private QuestData npcQuest;
    [TextArea] public string defaultText = "안녕하세요!";
    [TextArea] public string questCompleteText = "수고하셨어요!";
    [TextArea] public string questProgressText = "아직 다 못했군요. 계속 노력해봐요!";
    [TextArea] public string questOfferText = "오늘도 저를 도와줄 수 있나요?";

    private void Awake()
    {
        questGiver = GetComponent<NPCQuestGiver>();
        npcController = GetComponent<NPCController>();
    }

    private void Start()
    {
        npcName = npcController.npcName;
    }

    private void OnMouseDown()
    {
        if (questGiver == null || questGiver.questPool == null)
        {
            ShowDialogue(defaultText);
            return;
        }

        int hour = TimeManager.Instance.currentHour;
        int day = TimeManager.Instance.currentDay;
        QuestData quest = questGiver.questPool.GetRandomAvailableQuest(hour, day);

        if (quest == null)
        {
            ShowDialogue(defaultText);
            return;
        }

        //// 이미 완료한 퀘스트
        //if (QuestManager.Instance.HasCompletedQuest(quest.questID))
        //{
        //    ShowDialogue("이미 이 퀘스트를 완료했어요.");
        //    return;
        //}

        if (QuestManager.Instance.IsQuestActive(quest.questID)) // 퀘스트 진행중
        {
            QuestInstance instance = QuestManager.Instance.ActiveQuests.Find(q => q.questData.questID == quest.questID);

            if (instance.IsComplete)
            {
                ShowDialogue(questCompleteText);
            }
            else
            {
                ShowDialogue(questProgressText);
            }
        }
        else // 퀘스트 진행전
        {
            var fullText = $"{questOfferText} ({quest.questName})";
            ShowDialogue(fullText, () => {
                QuestManager.Instance.AcceptQuest(quest.questID);
            });
        }
    }

    private void ShowDialogue(string text, System.Action onOK = null)
    {
        NPCDialgoueUI.Instance.Show(npcName, text, onOK);
        UIManager.Instance.HidePopupImmediately();
    }
}
