using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PrincessScene1Controller : MonoBehaviour
{
    private string TAG = "[PrincessScene1Controller]";

    private UserInputManager inputManager;

    [SerializeField] public PrincessUIController princessUIController; // UI 변경용

    private Dialog currentDialog; // 현재 대사
    private int currentDialogId = 2; // 맨 첫 대사. dialogId는 2부터 시작
    private int optionNum = 1; // 옵션 선택 저장

    private bool isNextDialogReady = false; // 다음 대사 가능한지 판별
    private bool isTextSkipEnabled = false; // 대사 빨리 감기 가능한지 판별
    private bool isSkipRequested = false; // 대사 빨리감기

    private void Awake()
    {
        DialogTool.CsvRead("Dialog/Dialog"); // 대사 DB에서 가져 옴
    }

    private void OnEnable()
    {
        inputManager = UserInputManager.Instance; // 사용자 입력 받는 용도
    }

    private void Start()
    {
        GameManager.Instance.SetGameState(TAG, GameState.UI); // 입력을 UI모드로 변경

        StartCoroutine(StartScene1()); // 씬1 시작
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
            if (isNextDialogReady) // 다음 대사가 준비되었다면
            {
                bool isDialogEnd = false; // 다음 대사가 없는 경우
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

                    case DialogType.NONE: // 다음 대사가 없는 경우
                        Debug.Log($"{TAG} 스페이스 눌러봤자 소용 없음");
                        return;
                }

                if (isDialogEnd) // 다음 대사가 없는 경우
                {
                    Debug.Log($"{TAG} 대화 끝");
                    currentDialog.type = DialogType.NONE;
                    DisableDialogPanel();
                    OptionPanelDisable();
                    return;
                }

                StartCoroutine(StartConversation()); // 대사 출력
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
        currentDialog = DialogTool.dic[currentDialogId]; // 현재 대사 가져옴

        EnableDialogPanel();

        ChangeEmotionImage();

        OnStartTextPrint(); // todo 대사 출력 시 효과음 내기
        yield return TextTool.PrintTmpText(princessUIController.dialogText, currentDialog.GetText(), () => isSkipRequested);
        OnStopTextPrint();

        if (currentDialog.type == DialogType.CHOICE) OptionPanelEnable();
    }

    // dialog 화면 보이기
    private void EnableDialogPanel()
    {
        princessUIController.dialogPanel.SetActive(true);
    }

    // dialog 화면 숨기기
    private void DisableDialogPanel()
    {
        princessUIController.dialogPanel.SetActive(false);
    }

    // 감정 표현 변경하기
    private void ChangeEmotionImage()
    {
        Sprite newSprite;
        switch (currentDialog.emotion)
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

    // 대사 출력 시작
    private void OnStartTextPrint()
    {
        isNextDialogReady = false;
        isTextSkipEnabled = true;
        princessUIController.nextDialogBtn.SetActive(false);
        princessUIController.optionPanel.SetActive(false);
    }

    // 대사 출력 끝
    private void OnStopTextPrint()
    {
        isNextDialogReady = true;
        isTextSkipEnabled = false;
        if (currentDialog.type != DialogType.CHOICE) // 옵션 선택 대사일 경우 NextDialogBtn 안나옴
            princessUIController.nextDialogBtn.SetActive(true);
        isSkipRequested = false;
    }

    // 선택 옵션 창 열기
    private void OptionPanelEnable()
    {
        princessUIController.optionPanel.SetActive(true);
    }

    // 선택 옵션 창 닫기
    private void OptionPanelDisable()
    {
        princessUIController.optionPanel.SetActive(false);
    }
}
