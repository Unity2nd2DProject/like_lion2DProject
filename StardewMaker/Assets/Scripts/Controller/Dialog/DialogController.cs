using System.Collections;
using UnityEngine;

public class DialogController : MonoBehaviour
{
    private string TAG = "[DialogController]";

    private UserInputManager inputManager;

    private DialogView dialogView; // UI 부분

    private Dialog currentDialog; // 현재 대사
    private int optionNum = 1; // 옵션 선택 저장

    private bool isNextDialogReady = false; // 다음 대사 가능한지 판별
    private bool isTextSkipEnabled = false; // 대사 빨리 감기 가능한지 판별
    private bool isSkipRequested = false; // 대사 빨리감기
    bool isDialogEnd = true; // 대화가 끝났는지 판별

    void Awake()
    {
        DialogTool.CsvRead("Dialog/Dialog"); // 대사 DB에서 가져 옴
        dialogView = GetComponent<DialogView>();
    }

    private void OnEnable()
    {
        inputManager = UserInputManager.Instance; // 사용자 입력 받는 용도
        DialogTrigger.OnDialogRequested += StartDialog;
    }

    private void OnDisable()
    {
        DialogTrigger.OnDialogRequested -= StartDialog;
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
            if (isNextDialogReady) // 다음 대사가 준비되었다면
            {

                int currentDialogId = 0;
                switch (currentDialog.type)
                {
                    case DialogType.NORMAL: // 일반적인 대사 진행
                        currentDialogId = currentDialog.nextId; // 다음 대사 불러오기

                        // 다음 대사가 없는 경우
                        if (currentDialog.id == currentDialog.nextId) isDialogEnd = true;
                        break;

                    case DialogType.CHOICE: // 선택 옵션이 있는 대사 진행

                        // todo 옵션 선택해서 chooseNum 변경하기
                        Dialog checkedOptionDialog = DialogTool.dic[currentDialog.optionIdList[optionNum]];
                        currentDialogId = checkedOptionDialog.nextId;

                        // 옵션 선택 후 다음 대사 없는 경우 대화 끝
                        if (currentDialogId == DialogTool.dic[currentDialogId].nextId) isDialogEnd = true;
                        break;

                        // case DialogType.NONE: // 다음 대사가 없는 경우
                        //     Debug.Log($"{TAG} 스페이스 눌러봤자 소용 없음");
                        //     return;
                }

                if (isDialogEnd) // 다음 대사가 없는 경우
                {
                    Debug.Log($"{TAG} 대화 끝");
                    isNextDialogReady = false;
                    dialogView.EnableDialogPanel(false);
                    dialogView.EnableOptionPanel(false);
                    return;
                }

                StartDialog(currentDialogId); // 대사 출력
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

    public void StartDialog(int dialogId) //, Sprite portrait)
    {
        // todo portrait 사용할 방법

        if (isDialogEnd)
        {
            isDialogEnd = false;
            StartCoroutine(ConversationByDialogId(dialogId));
        }
    }

    public IEnumerator ConversationByDialogId(int id)
    {
        currentDialog = DialogTool.dic[id]; // id에 맞는 대사 가져옴

        dialogView.EnableDialogPanel(true);

        // ChangeEmotionImage(); // todo 임시. 아직 교체 안함

        OnStartTextPrint(); // todo 대사 출력 시 효과음 내기
        yield return TextTool.PrintTmpText(dialogView.dialogText, currentDialog.GetText(), () => isSkipRequested);
        OnStopTextPrint();

        if (currentDialog.type == DialogType.CHOICE) dialogView.EnableOptionPanel(true);
    }

    // 감정 표현 변경하기
    private void ChangeEmotionImage()
    {
        Sprite newSprite;
        switch (currentDialog.emotion)
        {
            case EmotionType.IDLE:
                newSprite = Resources.Load<Sprite>("Images/CharacterIllust/Father-0001");
                dialogView.ChangeCharaterImage(newSprite);
                break;
            case EmotionType.SAD:
                newSprite = Resources.Load<Sprite>("Images/CharacterIllust/Daughter-0001");
                dialogView.ChangeCharaterImage(newSprite);
                break;
        }
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
