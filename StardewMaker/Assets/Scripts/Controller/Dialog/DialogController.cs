using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DialogController : MonoBehaviour
{
    private string TAG = "[DialogController]";

    private UserInputManager inputManager;

    private DialogView dialogView; // UI 부분

    private Dialog currentDialog; // 현재 대사
    private NPCDialog currentNPCDailog; // 현재 NPC 정보
    private int selectedOptionNum = 0; // 옵션 선택 저장

    private bool isNextDialogReady = false; // 다음 대사 가능한지 판별
    private bool isTextSkipEnabled = false; // 대사 빨리 감기 가능한지 판별
    private bool isSkipRequested = false; // 대사 빨리감기
    private bool isDialogEnd = true; // 대화가 끝났는지 판별

    void Awake()
    {
        DialogTool.CsvRead("Dialog/Dialog"); // 대사 DB에서 가져 옴
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
            int currentDialogId = 0;
            switch (currentDialog.type)
            {
                case DialogType.NORMAL: // 일반적인 대사 진행
                    currentDialogId = currentDialog.nextId; // 다음 대사 id 저장

                    // 다음 대사가 없는 경우 대화 끝
                    if (currentDialog.id == currentDialog.nextId) isDialogEnd = true;
                    break;

                case DialogType.CHOICE: // 선택 옵션이 있는 대사 진행
                    dialogView.DeleteAllOptionButton(); // 옵션 리스트 삭제

                    Dialog checkedOptionDialog = DialogTool.dic[currentDialog.optionIdList[selectedOptionNum]];
                    currentDialogId = checkedOptionDialog.nextId; // 다음 대사 id 저장

                    // 옵션 선택 후 다음 대사 없는 경우 대화 끝
                    if (currentDialogId == DialogTool.dic[currentDialogId].nextId)
                    {
                        isDialogEnd = true;
                        // Debug.Log($"{TAG} isDialogEnd true, {checkedOptionDialog.ext1}");
                        switch (checkedOptionDialog.ext1)
                        {
                            case ExtType.EXIT:
                                Debug.Log($"{TAG} ExtType.EXIT");
                                break;
                            case ExtType.SHOP:
                                Debug.Log($"{TAG} ExtType.SHOP"); // todo 상점 열기
                                break;
                        }
                    }
                    break;
            }

            if (isDialogEnd) // 다음 대사가 없는 경우
            {
                Debug.Log($"{TAG} 대화 끝");
                isNextDialogReady = false;
                dialogView.EnableDialogPanel(false);
                dialogView.EnableOptionPanel(false);
                dialogView.EnableNPCImage(false);
            }
            else
            {
                isDialogEnd = true;
                StartDialog(currentDialogId); // 대사 출력
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

    // 다이얼로그 NPC 초기화
    public void InitDialog(NPCDialog npcDialog)
    {
        dialogView.SetNPCDialog(npcDialog);
        dialogView.EnableNPCImage(true);
        StartDialog(npcDialog.dialogId);
    }

    // 대화 시작
    public void StartDialog(int dialogId) //, Sprite portrait)
    {
        if (isDialogEnd) // 대사가 끝나있는 상태일때만 실행 가능
        {
            isDialogEnd = false;
            StartCoroutine(ConversationById(dialogId));
        }
    }

    public IEnumerator ConversationById(int id)
    {
        currentDialog = DialogTool.dic[id]; // id에 맞는 대사 가져옴

        dialogView.EnableDialogPanel(true);

        ChangeEmotionImage(currentDialog.emotion);

        ChangeCharacterName(currentDialog.name);

        OnStartTextPrint(); // todo 대사 출력 시 효과음 내기
        yield return TextTool.PrintTmpText(dialogView.dialogText, currentDialog.GetText(), () => isSkipRequested);
        OnStopTextPrint();

        // 옵션 생성 및 보여주기
        if (currentDialog.type == DialogType.CHOICE)
        {
            for (int i = 0; i < currentDialog.optionIdList.Count; i++)
            {
                int index = i; // 람다식 클로저 문제 방지
                GameObject go = dialogView.CreateOptionButton(DialogTool.dic[currentDialog.optionIdList[i]].GetText());
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
        isNextDialogReady = false;
        isTextSkipEnabled = true;
        dialogView.EnableNextDialogBtn(false);
        dialogView.EnableOptionPanel(false);
    }

    // 대사 출력 끝
    private void OnStopTextPrint()
    {
        isNextDialogReady = true;
        isTextSkipEnabled = false;
        if (currentDialog.type != DialogType.CHOICE) // 옵션 선택 대사일 경우 NextDialogBtn 안나옴
            dialogView.EnableNextDialogBtn(true);
        isSkipRequested = false;
    }
}
