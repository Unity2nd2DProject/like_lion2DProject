using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PrincessScene1Controller : MonoBehaviour
{
    private string TAG = "[PrincessScene1Controller]";

    private UserInputManager inputManager;

    [SerializeField] public PrincessUIController princessUIController;

    private bool isNextBtnEnabled = false;
    private bool isTextSkipEnabled = false;
    private bool isSkipRequested = false;

    private int cntText = 2;
    private int stateConversation = 0;

    private int chooseNum = 1;

    // private List<string> textList = new List<string> {
    //     "안녕 아빠! 잘 잤어요?",
    //     "콜록 콜록..",
    //     "오늘도 기침이 멈추질 않네..",
    //     "아빠는 오늘도 일하러 가야겠지.. 나는 아빠랑 더 있고싶은데.."
    // };

    // private List<string> textList1 = new List<string> {
    //     "정말 나랑 있어주는거에요?",
    //     "와! 기뻐라!!"
    // };

    // private List<string> textList2 = new List<string> {
    //     "아빤 내가 싫은가보다..",
    //     "흑흑.."
    // };

    // private List<List<string>> textlists = new() { };

    private void Awake()
    {
        ConversationTool.CsvRead("Conversation/Conversation");

        // textlists.Add(textList);
        // textlists.Add(textList1);
        // textlists.Add(textList2);

        LangTest();
    }

    private void LangTest()
    {
        // test
        // Debug.Log($"{TAG} name : {ConversationTool.GetName(2)}");
        // Debug.Log($"{TAG} emotion : {ConversationTool.GetEmotion(2)}");
        // Debug.Log($"{TAG} korean : {ConversationTool.GetKorean(2)}");
        // Debug.Log($"{TAG} english : {ConversationTool.GetEnglish(2)}");
    }

    private void OnEnable()
    {
        inputManager = UserInputManager.Instance;
    }

    private void Start()
    {
        GameManager.Instance.SetGameState(TAG, GameState.UI);

        StartCoroutine(StartScene1());
    }

    private void Update()
    {
        MoveInput();
        SpaceInput();
        ZInput();
    }

    private void MoveInput()
    {
        Vector2 moveInput = inputManager.inputActions.UI.Move.ReadValue<Vector2>();
    }

    private void SpaceInput()
    {
        if (inputManager.inputActions.UI.Space.WasPressedThisFrame())
        {
            if (isNextBtnEnabled)
            {
                switch (stateConversation)
                {
                    case 0:
                        StartCoroutine(StartConversation());
                        break;
                    case 1:
                        Debug.Log($"{TAG} 선택지 고르기 {chooseNum}");
                        StartCoroutine(StartConversation());
                        break;
                }
            }
        }
    }

    private void ZInput()
    {
        if (inputManager.inputActions.UI.Z.WasPressedThisFrame())
        {
            if (isTextSkipEnabled) // 대사가 출력중인 경우
            {
                isSkipRequested = true;
            }
        }
    }

    IEnumerator StartScene1()
    {
        // todo 페이드인
        yield return new WaitForSeconds(1);


        // 대화 시작
        yield return StartConversation();
    }

    IEnumerator StartConversation()
    {
        EnableDialog();

        OnStartTextPrint();

        ChangeEmotionImage();

        Dialogue dialogue = ConversationTool.dic[cntText++];

        int id = dialogue.id;
        int nextId = dialogue.nextId;
        string text = dialogue.korean;
        cntText = id + dialogue.nextId;

        // todo 대사 출력 시 효과음 내기
        yield return TextTool.PrintTmpText(princessUIController.dialogText, text, () => isSkipRequested);

        OnStopTextPrint();
        if (nextId == -1)
        {
            stateConversation++;
            ChooseOne();
        }
    }

    private void EnableDialog()
    {
        // todo 다이얼로그 박스 화면에 보이기
    }

    private void ChangeEmotionImage()
    {
        Sprite newSprite;
        switch (ConversationTool.GetEmotion(cntText))
        {
            case EmotionType.IDLE:
                newSprite = Resources.Load<Sprite>("Images/CharacterIllust/Father-0001");
                princessUIController.princessImage.GetComponent<Image>().sprite = newSprite;
                break;
            case EmotionType.SAD:
                newSprite = Resources.Load<Sprite>("Images/CharacterIllust/Daughter-0001");
                princessUIController.princessImage.GetComponent<Image>().sprite = newSprite;
                break;
        }
    }

    private void OnStartTextPrint()
    {
        isNextBtnEnabled = false;
        isTextSkipEnabled = true;
        princessUIController.nextBtn.SetActive(false);
    }

    private void OnStopTextPrint()
    {
        isNextBtnEnabled = true;
        isTextSkipEnabled = false;
        princessUIController.nextBtn.SetActive(true);
        isSkipRequested = false;
    }

    private void ChooseOne()
    {
        princessUIController.nextBtn.SetActive(false);
        princessUIController.chooseBox.SetActive(true);
        cntText = 0;
    }

}
