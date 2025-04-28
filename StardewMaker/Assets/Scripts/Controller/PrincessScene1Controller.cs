using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrincessScene1Controller : MonoBehaviour
{
    private string TAG = "[PrincessScene1Controller]";
    private UserInputManager inputManager;

    [SerializeField] private DialogController dialogController;
    [SerializeField] private GameObject buttonPanel;
    [SerializeField] private int testIndex;
    private NPCDialog npcDialog;

    private int introDialogId = 2;

    // todo introDialogId는 DB에서 가져와야 함

    void Awake()
    {
        GameManager.Instance.arrivalPointName = "InsideHomeFrontDoor"; //  FadeIn 실행하기 위함

        npcDialog = GetComponent<NPCDialog>();
    }

    private void OnEnable()
    {
        inputManager = UserInputManager.Instance; // 사용자 입력 받는 용도
        DialogController.OnButtonPanelRequested += EnableButtonPanel;
    }

    private void Start()
    {
        GameManager.Instance.SetGameState(TAG, GameState.UI); // 입력을 UI모드로 변경

        SetDialog();

        StartCoroutine(StartScene1()); // 씬1 시작
    }

    private void SetDialog()
    {
        // todo 아침인지 저녁인지 판별
        // todo 현재 날짜 확인

        // todo 어떤 대화를 할지 정해야 함

        // todo 낮과 밤에 따른 차이 두기?
        // Dictionary<ConditionType, int> conditionTypeValueDic = new();
        // conditionTypeValueDic[ConditionType.MOOD] = 5;
        // conditionTypeValueDic[ConditionType.VITALITY] = 5;
        // conditionTypeValueDic[ConditionType.HUNGER] = 5;
        // conditionTypeValueDic[ConditionType.TRUST] = 5;

        List<Dialog> vitalityDialogList = DialogTool.GetDialogListByCondition(StatType.VITALITY, DaughterManager.Instance.GetStats()); // VITALITY가 5일 때 대사들 가져옴
        npcDialog.currentDialogId = vitalityDialogList[Random.Range(0, vitalityDialogList.Count)].id;
        // Debug.Log($"{TAG} npcDialog.currentDialogId {npcDialog.currentDialogId}");

        // 게임 처음 시작 시 대화
        npcDialog.currentDialogId = introDialogId;
    }

    private void Update()
    {
        MoveInput();
    }

    void OnDisable()
    {
        DialogController.OnButtonPanelRequested -= EnableButtonPanel;
    }

    private void EnableButtonPanel(bool sw)
    {
        buttonPanel.SetActive(sw);
    }

    private void MoveInput()
    {
        Vector2 moveInput = inputManager.inputActions.UI.Move.ReadValue<Vector2>();
    }

    IEnumerator StartScene1()
    {
        // ArrivalPoint에서 FadeIn 진행
        yield return new WaitForSeconds(FadeManager.Instance.fadeDuration);

        // 대화 시작
        dialogController.InitDialog(npcDialog);
    }

    public void DialogTest()
    {
        npcDialog.currentDialogId = testIndex;
        dialogController.InitDialog(npcDialog);
    }

    public void ReadBook()
    {
        // Dictionary<ConditionType, int> conditionTypeValueDic = new();
        // conditionTypeValueDic[ConditionType.MOOD] = 8;
        // conditionTypeValueDic[ConditionType.VITALITY] = 5;
        // conditionTypeValueDic[ConditionType.HUNGER] = 5;
        // conditionTypeValueDic[ConditionType.TRUST] = 5;

        List<Dialog> bookDialogList = DialogTool.GetDialogListBySituation(SituationType.BOOK, DaughterManager.Instance.GetStats());
        npcDialog.currentDialogId = bookDialogList[Random.Range(0, bookDialogList.Count)].id;
        // npcDialog.currentDialogId = 103;
        dialogController.InitDialog(npcDialog);
    }
}
