using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrincessScene1Controller : MonoBehaviour
{
    private string TAG = "[PrincessScene1Controller]";
    private UserInputManager inputManager;

    public static event Action OnExitRequested;

    [SerializeField] private DialogController dialogController;
    [SerializeField] private GameObject normalMenu;
    [SerializeField] private GameObject scheduleMenu;
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
        DialogController.OnNormalMenuRequested += EnableNormalMenuPanel;
        DialogController.OnScheduleMenuRequested += EnableScheduleMenuPanel;
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

        List<Dialog> vitalityDialogList = DialogTool.GetDialogListByCondition(StatType.VITALITY, DaughterManager.Instance.GetStats()); // VITALITY가 5일 때 대사들 가져옴
        npcDialog.currentDialogId = vitalityDialogList[UnityEngine.Random.Range(0, vitalityDialogList.Count)].id;
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
        DialogController.OnNormalMenuRequested -= EnableNormalMenuPanel;
        DialogController.OnScheduleMenuRequested -= EnableScheduleMenuPanel;
    }

    public void EnableNormalMenuPanel(bool sw)
    {
        normalMenu.SetActive(sw);
    }

    public void EnableScheduleMenuPanel(bool sw)
    {
        normalMenu.SetActive(!sw);
        scheduleMenu.SetActive(sw);
    }

    public void Exit()
    {
        // todo 할 일들 저장
        OnExitRequested?.Invoke();
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
        StaminaUI.Instance.ConsumeStamina();

        npcDialog.currentDialogId = testIndex;
        dialogController.InitDialog(npcDialog);
    }

    public void ReadBook()
    {
        List<Dialog> bookDialogList = DialogTool.GetDialogListBySituation(SituationType.BOOK, DaughterManager.Instance.GetStats());
        npcDialog.currentDialogId = bookDialogList[UnityEngine.Random.Range(0, bookDialogList.Count)].id;
        // npcDialog.currentDialogId = 103;
        dialogController.InitDialog(npcDialog);
    }
}
