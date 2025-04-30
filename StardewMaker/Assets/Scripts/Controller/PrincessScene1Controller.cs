using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public enum scheduleType
{
    EXERCISE,
    HOME_WORK,
    PLAY,
    READ_BOOK,
    DRAWING,
    COOK,
    MUSIC,
    DOLL
}



public class PrincessScene1Controller : MonoBehaviour
{
    private string TAG = "[PrincessScene1Controller]";
    private UserInputManager inputManager;

    public static event Action OnExitRequested;

    [SerializeField] private DialogController dialogController;
    [SerializeField] private GameObject normalMenu;
    [SerializeField] private GameObject scheduleMenu;
    [SerializeField] private GameObject yesNoMenu;
    [SerializeField] private GameObject yesNoImage;
    [SerializeField] private int testIndex;
    private NPCDialog npcDialog;

    [SerializeField] private GameObject toggleButtonPrefab;
    private List<string> toggleButtonStringList = new();
    private List<ToggleButton> toggleButtonList = new();
    private int maxToggleButtonNum = 8;

    private int introDialogId = 2;

    // todo introDialogId는 DB에서 가져와야 함

    private Dictionary<scheduleType, string> scheduleTextDic = new(){
        {scheduleType.EXERCISE, "운동"},
        {scheduleType.HOME_WORK, "집안일"},
        {scheduleType.PLAY, "놀기"},
        {scheduleType.READ_BOOK, "독서"},
        {scheduleType.DRAWING, "그림그리기"},
        {scheduleType.COOK, "아빠와요리"},
        {scheduleType.MUSIC, "음악하기"},
        {scheduleType.DOLL, "인형놀이"},
    };

    void Awake()
    {
        GameManager.Instance.arrivalPointName = "InsideHomeFrontDoor"; //  FadeIn 실행하기 위함

        npcDialog = GetComponent<NPCDialog>();

        SetToggleButton();
    }

    private void SetToggleButton()
    {
        toggleButtonStringList.Add(scheduleTextDic[scheduleType.EXERCISE]);
        toggleButtonStringList.Add(scheduleTextDic[scheduleType.HOME_WORK]);
        toggleButtonStringList.Add(scheduleTextDic[scheduleType.PLAY]);

        // todo schedule 어떤게 가능한지 판별하기
        if (false) // 독서 가능할 때
        {
            toggleButtonStringList.Add(scheduleTextDic[scheduleType.READ_BOOK]);
        }
        if (true)
        {
            toggleButtonStringList.Add(scheduleTextDic[scheduleType.DRAWING]);
        }
        if (true)
        {
            toggleButtonStringList.Add(scheduleTextDic[scheduleType.COOK]);
        }
        if (true)
        {
            toggleButtonStringList.Add(scheduleTextDic[scheduleType.MUSIC]);
        }
        if (false)
        {
            toggleButtonStringList.Add(scheduleTextDic[scheduleType.DOLL]);
        }

        for (int i = 0; i < toggleButtonStringList.Count; i++)
        {
            string panelName = i < maxToggleButtonNum / 2 ? "Panel1" : "Panel2";
            ToggleButton toggleButton = Instantiate(toggleButtonPrefab, scheduleMenu.transform.Find(panelName)).GetComponent<ToggleButton>();
            toggleButton.SetText(toggleButtonStringList[i]);
            toggleButtonList.Add(toggleButton);
        }
    }

    private void OnEnable()
    {
        inputManager = UserInputManager.Instance; // 사용자 입력 받는 용도
        DialogController.OnNormalMenuRequested += EnableNormalMenuPanel;
        DialogController.OnScheduleMenuRequested += EnableScheduleMenuPanel;
        ToggleButton.OnToggleChangeRequested += CheckScheduleMenu;
    }

    private void Start()
    {
        GameManager.Instance.SetGameState(TAG, GameState.UI); // 입력을 UI모드로 변경

        SetDialog();

        StartCoroutine(StartScene1()); // 씬1 시작
    }



    private void Update()
    {
        MoveInput();
    }

    void OnDisable()
    {
        DialogController.OnNormalMenuRequested -= EnableNormalMenuPanel;
        DialogController.OnScheduleMenuRequested -= EnableScheduleMenuPanel;
        ToggleButton.OnToggleChangeRequested -= CheckScheduleMenu;
    }

    private void CheckScheduleMenu(string text, bool isOn)
    {
        // Debug.Log($"{TAG} CheckScheduleMenu {text} {isOn}");
        int cnt = 0;
        for (int i = 0; i < toggleButtonList.Count; i++)
        {
            if (toggleButtonList[i].toggle.isOn) cnt++;
            // Debug.Log($"{TAG} cnt {cnt}");
            if (cnt >= 2)
            {
                yesNoMenu.SetActive(true);
                yesNoImage.SetActive(true);
                return;
            }
        }
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
        for (int i = 0; i < toggleButtonList.Count; i++)
        {
            if (toggleButtonList[i].toggle.isOn)
            {
                Debug.Log($"{TAG} toggleButtonList {i} isOn");
            }
        }
        OnExitRequested?.Invoke();
    }

    public void NoExit()
    {
        for (int i = 0; i < toggleButtonList.Count; i++)
        {
            toggleButtonList[i].toggle.isOn = false;
        }
        yesNoMenu.SetActive(false);
        yesNoImage.SetActive(false);
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
        StaminaManager.Instance.ConsumeStamina();
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
