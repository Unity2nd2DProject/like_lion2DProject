using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using NPC;
using Unity.VisualScripting;

public class DialogueController : MonoBehaviour
{
    [SerializeField] private GameObject textBox;
    [SerializeField] private Image npcImage;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI dialogueText;

    [Header("Buttons")]
    [SerializeField] private GameObject buttonGrid;
    [SerializeField] private GameObject buttonPrefab;
    private List<Button> dialogueButtons = new List<Button>();

    [Header("Typing Effect")]
    private bool isTyping = false;
    [SerializeField] private Button nextButton;
    [SerializeField] private float skipCooldown = 0.2f;

    // 내부 데이터
    private NPCDialogue currentDialogue;
    private NpcSpritesSet currentNpcSpriteSet;
    private NPCController currentNPC;
    private QuestDataSO currentQuest;

    private bool waitingForNext = false;

    [Header("Quest UI")]
    [SerializeField] private QuestDetailPopupUI questDetailPopupUI;

    public void SetDialogue(NPCDialogue npcDialogue, NpcSpritesSet npcSpriteSet, NPCController npc)
    {
        this.currentDialogue = npcDialogue;
        this.currentNpcSpriteSet = npcSpriteSet;
        currentNPC = npc;

        // NPC 이름, 기본 표정 세팅
        nameText.text = currentDialogue.name;
        npcImage.sprite = currentNpcSpriteSet.neutral;

        nextButton.onClick.AddListener(OnNextButtonPressed);

        foreach (var btn in dialogueButtons)
            Destroy(btn.gameObject);

        dialogueButtons.Clear();

        gameObject.SetActive(true);

        StartCoroutine(PlayRandomDialogue(DialogueSequenceType.Greeting, ShowMainButtons));
    }
    private IEnumerator PlayRandomDialogue(DialogueSequenceType type, UnityEngine.Events.UnityAction doAfterTalk)
    {
        List<DialogueSequence> chatSequences = currentDialogue.dialogues.FindAll(seq => seq.sequenceType == type);

        if (chatSequences.Count == 0)
        {
            Debug.LogWarning($"No chat sequences of type {type} found.");
            doAfterTalk?.Invoke();
            yield break;
        }

        DialogueSequence randomSeq = chatSequences[Random.Range(0, chatSequences.Count)];

        foreach (var line in randomSeq.lines)
        {
            // 발화자 이름 표시
            nameText.text = line.speaker;

            // 표정 세팅 (있으면)
            if (line.actions != null && line.actions.useExpression)
                npcImage.GetComponent<Image>().sprite = GetExpressionSprite(line.actions.expression);

            // 타자 효과 시작
            yield return StartCoroutine(TypeText(line.text));

            // 다음 라인으로 넘어가기 전 플레이어 입력 대기 (예: 클릭 or 키 입력)
            yield return StartCoroutine(WaitForNextLineInput());
        }

        doAfterTalk?.Invoke();
    }

    #region 다이얼로그 버튼 관련 
    // Button
    private void CreateMainButtons()
    {
        // 대화 버튼
        CreateButton(buttonPrefab, "대화하기", OnTalkButton);

        // 상점 가능하면
        if (currentNPC.shopAvailable)
            CreateButton(buttonPrefab, "상점", OnTradeButton);

        // 퀘스트 가능하면
        if (currentNPC.questAvailable)
            CreateButton(buttonPrefab, "퀘스트", OnQuestButton);

        // 떠나기 버튼
        CreateButton(buttonPrefab, "나가기", OnLeaveButton);
    }

    private void CreateQuestButtons()
    {
        // 퀘스트 수락 버튼
        CreateButton(buttonPrefab, "수락", OnQuestAcceptButton);
        // 퀘스트 거절 버튼
        CreateButton(buttonPrefab, "거절", OnQuestDeclineButton);
    }

    private void ResetButtons()
    {
        foreach (Transform child in buttonGrid.transform)
            Destroy(child.gameObject);

        dialogueButtons.Clear();
    }

    private void CreateButton(GameObject prefab, string label, UnityEngine.Events.UnityAction onClick)
    {
        GameObject btnObj = Instantiate(prefab, buttonGrid.transform);        
        Button btn = btnObj.GetComponent<Button>();
        dialogueButtons.Add(btn);
        TextMeshProUGUI text = btnObj.GetComponentInChildren<TextMeshProUGUI>();
        text.text = label;
        btn.onClick.AddListener(onClick);
    }

    private void OnTalkButton()
    {
        HideButtons();
        StartCoroutine(PlayRandomDialogue(DialogueSequenceType.Chat,BackToMain));
    }

    private void OnTradeButton()
    {
        HideButtons();
        Debug.Log("상점 열기 로직 호출");     
        gameObject.SetActive(false);
        currentNPC.vendor.OpenShop();
    }

    private void OnQuestButton()
    {
        HideButtons();
        Debug.Log("퀘스트 대화 시작");
        if(!currentNPC.questGiver.isQuestGiven && currentNPC.questGiver.todaysQuest != null)
        {
            Debug.Log(currentNPC.questGiver.isQuestGiven);
            StartCoroutine(PlayRandomDialogue(DialogueSequenceType.QuestOffer, ShowQuest));
        }
        else
        {
            StartCoroutine(PlayRandomDialogue(DialogueSequenceType.QuestUnavailable, BackToMain));
        }
    }

    private void OnQuestAcceptButton()
    {
        currentNPC.questGiver.AcceptQuest();
        questDetailPopupUI.Hide();
        HideButtons();
        StartCoroutine(PlayRandomDialogue(DialogueSequenceType.QuestAccept, BackToMain));
    }

    private void OnQuestDeclineButton()
    {
        questDetailPopupUI.Hide();
        HideButtons();
        StartCoroutine(PlayRandomDialogue(DialogueSequenceType.QuestDecline, EndBuissness));
    }

    private void OnLeaveButton()
    {
        HideButtons();
        StartCoroutine(PlayRandomDialogue(DialogueSequenceType.Farewell, CloseDialogue));
    }

    private void ShowMainButtons()
    {
        ResetButtons();
        CreateMainButtons();
        buttonGrid.SetActive(true);
    }

    private void ShowQuestButton()
    {
        Debug.Log("퀘스트 버튼 표시");
        ResetButtons();
        CreateQuestButtons();
        buttonGrid.SetActive(true);
    }

    private void HideButtons()
    {
        buttonGrid.SetActive(false);
    }

    public void BackToMain()
    {
        // 메인 대화 화면으로 복귀
        ShowMainButtons();
    }

    public void ShowQuest()
    {
        Debug.Log("퀘스트 상세 팝업 표시");
        questDetailPopupUI.Show(currentNPC.questGiver.todaysQuest);
        ShowQuestButton();
    }

    public void EndBuissness()
    {
        StartCoroutine(PlayRandomDialogue(DialogueSequenceType.BusinessEnd, BackToMain));
    }

    public void CloseDialogue()
    {
        gameObject.SetActive(false);
    }
    #endregion

    #region 타자 효과 관련
    private IEnumerator TypeText(string text)
    {
        isTyping = true;
        dialogueText.text = "";

        foreach (char c in text)
        {
            dialogueText.text += c;

            // 도중에 스킵 요청이 들어오면 즉시 완성
            if (!isTyping)
            {
                dialogueText.text = text;
                break;
            }

            yield return new WaitForSeconds(0.03f);
        }

        isTyping = false;
    }

    private IEnumerator WaitForNextLineInput()
    {
        waitingForNext = true;
        nextButton.interactable = true;

        // 사용자가 버튼을 누를 때까지 대기
        while (waitingForNext)
            yield return null;

        // 다음 라인으로 이동하기 전에 잠시 대기 (연타 방지)
        yield return new WaitForSeconds(0.2f);
    }

    private Sprite GetExpressionSprite(NpcEmotion emotion)
    {
        switch (emotion)
        {
            case NpcEmotion.Happy: return currentNpcSpriteSet.happy;
            case NpcEmotion.Sad: return currentNpcSpriteSet.sad;
            case NpcEmotion.Surprised: return currentNpcSpriteSet.surprised;
            default: return currentNpcSpriteSet.neutral;
        }
    }

    public void OnNextButtonPressed()
    {
        // 대사 타이핑 중이라면 → 즉시 완성
        if (isTyping)
        {
            isTyping = false;
            return;
        }

        // 이미 완성된 상태라면 → 다음 문장으로 진행
        if (waitingForNext)
        {
            waitingForNext = false;
            nextButton.interactable = false;
        }
    }
    #endregion
}