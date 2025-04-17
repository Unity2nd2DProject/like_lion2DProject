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

    private int cntText = 0;

    private List<string> textList = new List<string> {
        "안녕 아빠! 잘 잤어요?",
        "콜록 콜록..",
        "오늘도 기침이 멈추질 않네..",
        "아빠는 오늘도 일하러 가야겠지.. 나는 아빠랑 더 있고싶은데.."
    };

    private void Awake()
    {
        ConversationTool.CsvRead("Conversation/Conversation");

        LangTest();
    }

    private void LangTest()
    {
        // test
        Debug.Log($"{TAG} text : {textList[0]}");
        Debug.Log($"{TAG} name : {ConversationTool.GetName(textList[0])}");
        Debug.Log($"{TAG} emotion : {ConversationTool.GetEmotion(textList[0])}");
        Debug.Log($"{TAG} korean : {ConversationTool.GetKorean(textList[0])}");
        Debug.Log($"{TAG} english : {ConversationTool.GetEnglish(textList[0])}");
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
        SpaceInput();
        ZInput();
    }

    private void SpaceInput()
    {
        if (inputManager.inputActions.UI.Space.WasPressedThisFrame())
        {
            if (isNextBtnEnabled)
            {
                if (cntText >= textList.Count)
                {
                    Debug.Log("대사 없음");
                }
                else
                {
                    StartCoroutine(StartConversation());
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
        OnStartTextPrint();

        ChangeEmotionImage();

        // todo 대사 출력 시 효과음 내기
        yield return TextTool.PrintTmpText(princessUIController.dialogText, textList[cntText++], () => isSkipRequested);

        OnStopTextPrint();
    }

    private void ChangeEmotionImage()
    {
        Sprite newSprite;
        switch (ConversationTool.GetEmotion(textList[cntText]))
        {
            case (int)EmotionType.IDLE:
                newSprite = Resources.Load<Sprite>("Images/CharacterIllust/Father-0001");
                princessUIController.princessImage.GetComponent<Image>().sprite = newSprite;
                break;
            case (int)EmotionType.SAD:
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

}
