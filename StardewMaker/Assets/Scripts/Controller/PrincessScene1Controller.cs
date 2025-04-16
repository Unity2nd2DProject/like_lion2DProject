using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrincessScene1Controller : MonoBehaviour
{
    private string TAG = "[PrincessScene1Controller]";

    private UserInputManager inputManager;

    [SerializeField] public PrincessUIController princessUIController;

    private bool isEnterEnabled = false;
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
            if (isEnterEnabled)
            {
                if(cntText >= textList.Count)
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
        // todo 표정 변화. 표정변화도 csv에 들어가야 할 듯
        // todo 대사 출력 시 효과음 내기
        yield return TextTool.PrintTmpText(princessUIController.dialogText, textList[cntText++], () => isSkipRequested);
        
        OnStopTextPrint();
    }

    private void OnStartTextPrint()
    {
        // todo 화면에서 엔터 안 보이게
        isEnterEnabled = false;
        isTextSkipEnabled = true;
    }

    private void OnStopTextPrint()
    {
        // todo 화면에서 엔터 보이게
        isEnterEnabled = true;
        isTextSkipEnabled = false;
        isSkipRequested = false;
    }

}
