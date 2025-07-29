using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class NPCInteraction : MonoBehaviour
{
    private NPCQuestGiver questGiver;
    private NPCController npcController;

    [Header("Interaction")]
    [SerializeField] private float interactionRange = 2.5f;

    [Header("Dialogue")]
    private string npcName;
    private QuestData npcQuest;
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
        npcName = npcController.npcName;
    }

    private void OnMouseDown()
    {
        Transform playerTransform = PlayerController.Instance.transform;
        float distance = Vector2.Distance(transform.position, playerTransform.position);

        if (distance > interactionRange)
        {
            return;
        }

        if (questGiver.questPool == null )
        {
            ShowDialogue(defaultText);
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
        QuestData quest = questGiver.questPool.GetRandomAvailableQuest(hour, day);

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

    private void ShowDialogue(string text, QuestData questData = null, System.Action onOK = null)
    {
        NPCDialgoueUI.Instance.Show(npcImage, npcName, text, questData, onOK);
        UIManager.Instance.HidePopupImmediately();
    }
}
