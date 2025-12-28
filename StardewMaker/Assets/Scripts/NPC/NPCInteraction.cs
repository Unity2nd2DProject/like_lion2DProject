using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class NPCInteraction : MonoBehaviour
{
    private NPCQuestGiver questGiver;
    private NPCController npcController;

    [Header("Interaction")]
    [SerializeField] private float interactionRange = 2.5f;

    [Header("Dialogue")]
    private NPC.NpcId npcName;
    private QuestDataSO npcQuest;
    [SerializeField] Sprite npcImage;
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
        npcName = npcController.npcID;
    }

    /*
    private void OnMouseDown()
    {
        Debug.Log($"[NPCInteraction] {npcName} 클릭됨");
        Transform playerTransform = PlayerController.Instance.transform;
        float distance = Vector2.Distance(transform.position, playerTransform.position);

        if (distance > interactionRange)
        {
            return;
        }
        else
        {
            // DialogueManager.Instance.StartDialogue();
        }
        
        QuestManager.Instance.ReportAction(QuestTargetType.GreetedToNPC);

        if (questGiver.questPool == null)
        {
            ShowDialogue(defaultText);
            return;
        }

        // 퀘스트 진행 여부 우선 확인
        QuestInstance instance = QuestManager.Instance.ActiveQuests
            .Find(q => questGiver.questPool.dailyQuests.Contains(q.questData));

        if (instance != null)
        {
            if (instance.IsComplete)
            {
                if (instance.giverNpcName == npcName)
                {
                    ShowDialogue(questCompleteText, null, () => {
                        QuestManager.Instance.CompleteQuest(instance);
                    });
                }
                else
                {
                    ShowDialogue(defaultText);
                }
            }
            else
            {
                ShowDialogue(questProgressText);
            }

            return;
        }

        // 아직 수락 안 한 경우 (수락 가능한 시간 체크)
        int hour = TimeManager.Instance.currentHour;
        int day = TimeManager.Instance.currentDay;
        QuestDataSO quest = questGiver.questPool.GetRandomAvailableQuest(hour, day);

        // 오늘 이미 완료한 daily quest인지 먼저 확인
        bool alreadyCompletedToday = questGiver.questPool.dailyQuests.Exists(q =>
            QuestManager.Instance.HasCompletedQuest(q.questID)
        );

        if (alreadyCompletedToday)
        {
            ShowDialogue(defaultText);
            return;
        }

        if (quest != null)
        {
            ShowDialogue(questOfferText, quest, () => {
                QuestManager.Instance.AcceptQuest(quest.questID, npcName);
            });
        }
        else
        {
            ShowDialogue(defaultText);
        }
       
}
     */

    private void ShowDialogue(string text, QuestDataSO questData = null, System.Action onOK = null)
    {
        NPCDialgoueUI_legacy.Instance.Show(npcImage, npcName, text, questData, onOK);
        UIManager.Instance.HidePopupImmediately();
    }
}
