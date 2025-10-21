using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using NPC;

public class DialogueController : MonoBehaviour
{
    [SerializeField] private GameObject textBox;
    [SerializeField] private Image npcImage;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI dialogueText;

    [Header("Buttons")]
    [SerializeField] private GameObject buttonGrid;
    [SerializeField] private GameObject buttonPrefab;

    [Header("Typing Effect")]
    private Coroutine typingCoroutine;
    private bool isTyping = false;
    [SerializeField] private Button skipButton; // 즉시 완성용 버튼
    [SerializeField] private float skipCooldown = 0.2f;

    private NPCDialogue currentDialogue;
    private NpcSpritesSet currentNpcSpriteSet;
    private bool shopAvailable;
    private bool questAvailable;

    public void Start()
    {
        Hide();
    }

    public void SetDialogue(NPCDialogue npcDialogue, NpcSpritesSet npcSpriteSet, bool shopAvailable, bool questAvailable)
    {
        this.currentDialogue = npcDialogue;
        this.currentNpcSpriteSet = npcSpriteSet;
        this.shopAvailable = shopAvailable;
        this.questAvailable = questAvailable;

        StartDialogue();
    }

    public void StartDialogue()
    {
        if (currentDialogue == null)
        {
            Debug.LogError("DialogueController: No dialogue set.");
            return;
        }

        gameObject.SetActive(true);

        // NPC 이름, 기본 표정 세팅
        nameText.text = currentDialogue.name;
        npcImage.sprite = currentNpcSpriteSet.neutral;

        StartCoroutine(StartDialogueCoroutine());
    }

    private IEnumerator StartDialogueCoroutine()
    {
        yield return StartCoroutine(ShowRandomChat(DialogueSequenceType.Greeting, CreateButtons));

        // 인사말 끝난 후 버튼 생성
        
    }

    private IEnumerator ShowRandomChat(DialogueSequenceType type, UnityEngine.Events.UnityAction doAfterTalk)
    {
        List<DialogueSequence> chatSequences = currentDialogue.dialogues.FindAll(seq => seq.sequenceType == type);

        if (chatSequences.Count == 0)
        {
            Debug.LogWarning($"No chat sequences of type {type} found.");
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
    }
    private void CreateButtons()
    {
        // 이전 버튼들 제거
        for (int i = buttonGrid.transform.childCount - 1; i >= 0; i--)
        {
            Destroy(buttonGrid.transform.GetChild(i).gameObject);
        }

        // 대화 버튼
        CreateButton(buttonPrefab, "대화하기", OnTalkButton);

        // 상점 가능하면
        if (shopAvailable)
            CreateButton(buttonPrefab, "상점", OnTradeButton);

        // 퀘스트 가능하면
        if (questAvailable)
            CreateButton(buttonPrefab, "퀘스트", OnQuestButton);

        // 떠나기 버튼
        CreateButton(buttonPrefab, "나가기", OnLeaveButton);
    }

    private void CreateButton(GameObject prefab, string label, UnityEngine.Events.UnityAction onClick)
    {
        GameObject btnObj = Instantiate(prefab, buttonGrid.transform);
        Button btn = btnObj.GetComponent<Button>();
        TextMeshProUGUI text = btnObj.GetComponentInChildren<TextMeshProUGUI>();
        text.text = label;
        btn.onClick.AddListener(onClick);
    }

    private void OnTalkButton()
    {
        StartCoroutine(ShowRandomChat(DialogueSequenceType.Chat,BackToMain));
    }

    private void OnTradeButton()
    {
        Debug.Log("상점 열기 로직 호출");
    }

    private void OnQuestButton()
    {
        Debug.Log("퀘스트 대화 시작");
        // NPCQuestGiver questGiver = ...; // 현재 NPC의 QuestGiver 컴포넌트 참조해야할듯..? 
    }

    private void OnLeaveButton()
    {
        Hide();
    }

    private void BackToMain()
    {
        // 메인 대화 화면으로 복귀
        CreateButtons();
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

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
        // 버튼 연타 방지: 잠깐 비활성화
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

    public void OnSkipButtonPressed()
    {
        if (!isTyping) return;

        isTyping = false; // TypeText 코루틴에서 감지 → 즉시 완성
        skipButton.interactable = false; // 연타 방지

        StartCoroutine(EnableSkipButtonAfterCooldown());
    }

    private IEnumerator EnableSkipButtonAfterCooldown()
    {
        yield return new WaitForSeconds(skipCooldown);
        skipButton.interactable = true;
    }
}