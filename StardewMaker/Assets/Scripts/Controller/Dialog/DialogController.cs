using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogController : MonoBehaviour
{
    private string TAG = "[DialogController]";

    private UserInputManager inputManager;

    // 메뉴 열기, 상점 열기 등 이벤트 전달
    public static event Action<bool> OnNormalMenuRequested;
    public static event Action<bool> OnScheduleMenuRequested;
    public static event Action OnShopRequested;
    public static event Action OnExitRequested;
    public static event Action OnNextDayRequested;

    private DialogView dialogView; // UI 부분

    private NPCDialog currentNPCDialog;
    private Dialog currentDialog; // 현재 대사
    private int selectedOptionNum = 0; // 옵션 선택 저장

    private bool isNextDialogReady = false; // 다음 대사 가능한지 판별
    private bool isTextSkipEnabled = false; // 대사 빨리 감기 가능한지 판별
    private bool isSkipRequested = false; // 대사 빨리감기
    private bool isDialogEnd = true; // 대화가 끝났는지 판별

    public StatType wantedConditionType; // 딸이 원하는 스케줄 타입

    void Awake()
    {
        DialogTool.Init();
        dialogView = GetComponent<DialogView>();
    }

    private void OnEnable()
    {
        inputManager = UserInputManager.Instance; // 사용자 입력 받는 용도
        DialogTrigger.OnDialogRequested += InitDialog;
    }

    private void OnDisable()
    {
        DialogTrigger.OnDialogRequested -= InitDialog;
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
            OnClickSpaceInput();
        }
    }

    public void OnClickSpaceInput()
    {
        if (isNextDialogReady) // 다음 대사가 준비되었다면
        {
            int nextDialogId = 0;
            ExtType extType = ExtType.NONE;

            // 현재 띄워져 있는 Dialog를 기반으로 다음 Dialog Id를 nextDialogId에 저장해서 대사 출력
            switch (currentDialog.type)
            {
                case DialogType.NORMAL: // 일반적인 대사 진행
                case DialogType.CONDITION: // 일반적인 대사 진행
                    nextDialogId = currentDialog.nextId; // 다음 대사 id 저장

                    // 다음 대사가 없는 경우 대화 끝
                    if (currentDialog.id == currentDialog.nextId)
                    {
                        isDialogEnd = true;
                        extType = currentDialog.ext1;
                    }
                    break;

                case DialogType.CHOICE: // 선택 옵션이 있는 대사 진행
                    dialogView.DeleteAllOptionButton(); // 옵션 리스트 삭제

                    Dialog checkedOptionDialog = DialogTool.dialogDic[currentDialog.optionIdList[selectedOptionNum]];
                    nextDialogId = checkedOptionDialog.nextId; // 다음 대사 id 저장

                    // 다음 대사가 없는 경우 대화 끝
                    if (checkedOptionDialog.id == checkedOptionDialog.nextId)
                    {
                        isDialogEnd = true;
                        extType = currentDialog.ext1;
                    }
                    break;

                case DialogType.MULTI: // Condition에 따른 멀티 대사 진행
                    int selectedNum = currentDialog.IsConditionMet(DaughterManager.Instance.GetStats()) ? (int)MultiOptionIdType.FIRST : (int)MultiOptionIdType.SECOND;
                    Dialog selectedDialog = DialogTool.dialogDic[currentDialog.optionIdList[selectedNum]];
                    nextDialogId = selectedDialog.id; // 다음 대사 id 저장

                    // 다음 대사가 없는 경우 대화 끝. optionIdList[0]이 0일 때 동작
                    if (currentDialog.id == nextDialogId)
                    {
                        isDialogEnd = true;
                        extType = currentDialog.ext1;
                    }
                    break;
            }

            // target condition value 변경하기
            if (extType != ExtType.WILL) currentDialog.SetTarget(DaughterManager.Instance.GetStats());

            if (isDialogEnd) // 다음 대사가 없는 경우
            {
                Debug.Log($"{TAG} 대화 끝");
                isNextDialogReady = false;
                dialogView.EnableDialogPanel(false);
                dialogView.EnableOptionPanel(false);

                switch (extType)
                {
                    case ExtType.ACT: // 집에서 행동

                        // PRINCESS인 경우 캐릭터 이미지 IDLE로 지속
                        if (currentNPCDialog.nameType == NameType.PRINCESS) ChangeEmotionImage(EmotionType.IDLE);
                        else dialogView.EnableNPCImage(false);

                        Debug.Log($"{TAG} ExtType.ACT");
                        OnNormalMenuRequested?.Invoke(true);
                        break;
                    case ExtType.EXIT: // 집에서 나가기 -> 스케줄 메뉴
                        Debug.Log($"{TAG} ExtType.EXIT");
                        if (currentNPCDialog.nameType == NameType.PRINCESS) OnScheduleMenuRequested?.Invoke(true);
                        // todo 상점에서 나가기
                        break;
                    case ExtType.SHOP: // 상점 열기
                        Debug.Log($"{TAG} ExtType.SHOP"); // todo 상점 열기
                        OnShopRequested?.Invoke();
                        break;
                    case ExtType.WILL:
                        GameManager.Instance.wantedDialog = currentDialog;
                        Debug.Log($"{TAG} ExtType.WILL {GameManager.Instance.wantedDialog.scheduleType}");
                        OnNormalMenuRequested?.Invoke(true);
                        break;
                    case ExtType.SLEEP:
                        Debug.Log($"{TAG} 잠자기 진행해야 함");
                        FadeManager.Instance.FadeOut(() =>
                        {
                            TimeManager.Instance.AdvanceDay();
                            FadeManager.Instance.FadeIn();
                            OnNextDayRequested?.Invoke();
                        });
                        break;
                }
            }
            else
            {
                isDialogEnd = true;
                StartDialog(nextDialogId); // 대사 출력
            }

        }
    }

    public void ZInput()
    {
        if (inputManager.inputActions.UI.Z.WasPressedThisFrame())
        {
            if (isTextSkipEnabled) // 대사가 출력중인 경우
            {
                isSkipRequested = true;
            }
        }
    }

    // 다이얼로그 NPC 처음 시작
    public void InitDialog(NPCDialog npcDialog)
    {
        currentNPCDialog = npcDialog;
        dialogView.SetNPCDialog(npcDialog);
        dialogView.EnableNPCImage(true);
        StartDialog(npcDialog.currentDialogId);
    }

    // 대화 시작
    public void StartDialog(int dialogId) //, Sprite portrait)
    {
        if (isDialogEnd) // 대사가 끝나있는 상태일때만 실행 가능
        {
            isDialogEnd = false;
            OnNormalMenuRequested?.Invoke(false);
            StartCoroutine(ConversationById(dialogId));
        }
    }

    public IEnumerator ConversationById(int id)
    {
        currentDialog = DialogTool.dialogDic[id]; // id에 맞는 대사 가져옴

        dialogView.EnableDialogPanel(true);

        ChangeEmotionImage(currentDialog.emotion);

        ChangeCharacterName(currentDialog.GetName());

        OnStartTextPrint();
        yield return TextTool.PrintTmpText(dialogView.dialogText, currentDialog.GetText(), () => isSkipRequested);
        OnStopTextPrint();

        // 옵션 생성 및 보여주기
        if (currentDialog.type == DialogType.CHOICE)
        {
            for (int i = 0; i < currentDialog.optionIdList.Count; i++)
            {
                int index = i; // 람다식 클로저 문제 방지
                GameObject go = dialogView.CreateOptionButton(DialogTool.dialogDic[currentDialog.optionIdList[i]].GetText());
                go.GetComponent<Button>().onClick.AddListener(() => OnClickOption(index));
            }
            dialogView.EnableOptionPanel(true);
        }
    }

    // 감정 표현 변경하기
    private void ChangeEmotionImage(EmotionType emotionType)
    {
        dialogView.ChangeCharaterImage(emotionType);
    }

    // 해당 대사의 캐릭터 이름으로 변경
    private void ChangeCharacterName(string name)
    {
        dialogView.SetCharacterName(name);
    }

    // 옵션 클릭시 실행
    private void OnClickOption(int i)
    {
        // Debug.Log($"{TAG} OnClickOption {i}");
        selectedOptionNum = i;
        OnClickSpaceInput();
    }

    // 대사 출력 시작
    private void OnStartTextPrint()
    {
        SoundManager.Instance.PlaySfxDialog(Volume.MEDIUM);
        selectedOptionNum = 0; // 선택된 옵션 초기화
        isNextDialogReady = false;
        isTextSkipEnabled = true;
        dialogView.EnableNextDialogBtn(false);
        dialogView.EnableOptionPanel(false);
    }

    // 대사 출력 끝
    private void OnStopTextPrint()
    {
        SoundManager.Instance.StopSfx();
        isNextDialogReady = true;
        isTextSkipEnabled = false;
        if (currentDialog.type != DialogType.CHOICE) // 옵션 선택 대사일 경우 NextDialogBtn 안나옴
            dialogView.EnableNextDialogBtn(true);
        isSkipRequested = false;
    }
}
