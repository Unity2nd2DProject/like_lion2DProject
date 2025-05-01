using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PrincessScene1Controller : MonoBehaviour
{
    private string TAG = "[PrincessScene1Controller]";
    private UserInputManager inputManager;
    public static event Action OnExitRequested; // 홈에서 나가기

    [Header("GameObject들 관리")]
    [SerializeField] private DialogController dialogController;
    [SerializeField] private GameObject normalMenu;
    [SerializeField] private GameObject scheduleMenu;
    [SerializeField] private GameObject dialogMenu;
    [SerializeField] private GameObject yesNoMenu;
    [SerializeField] private GameObject yesNoImage;
    [SerializeField] private GameObject exitAndSleepButton;

    [Space]
    [SerializeField] private GameObject toggleButtonPrefab;
    [SerializeField] private int dialogTestIndex;

    private NPCDialog npcDialog;
    private BackgroundController backgroundController;

    private List<ToggleButton> toggleButtonList = new();
    private int maxToggleButtonNum = 8;
    private bool isDay = true;

    private List<DialogSubjectButton> dialogSubjectButtonList = new();
    [SerializeField] private GameObject dialogSubjectButtonPrefab;

    private int introDialogId = 2; // todo introDialogId는 DB에서 가져와야 함

    private Dictionary<ScheduleType, string> scheduleTextDic = new(){
        {ScheduleType.EXERCISE, "운동"},
        {ScheduleType.HOME_WORK, "집안일"},
        {ScheduleType.PLAY, "놀기"},
        {ScheduleType.READ_BOOK, "독서"},
        {ScheduleType.DRAWING, "그림그리기"},
        {ScheduleType.COOK, "아빠와요리"},
        {ScheduleType.MUSIC, "음악하기"},
        {ScheduleType.DOLL, "인형놀이"},
    };

    private Dictionary<SituationType, string> dialogSubjectDic = new(){
        {SituationType.WANT_TO_DO, "하고싶은것"},
        {SituationType.WANT_TO_BE, "되고싶은것"},
        {SituationType.WANT_TO_EAT, "먹고싶은것"},
    };

    void Awake()
    {
        GameManager.Instance.arrivalPointName = "InsideHomeFrontDoor"; //  FadeIn 실행하기 위함

        npcDialog = GetComponent<NPCDialog>();
        backgroundController = GetComponent<BackgroundController>();

        SetAvailableSchedule(); // 스케줄 정하기

        // SoundManager.Instance.PlayBGM1(Volume.MEDIUM); // 동작 함
    }

    private void OnEnable()
    {
        inputManager = UserInputManager.Instance; // 사용자 입력 받는 용도
        DialogController.OnNormalMenuRequested += EnableNormalMenuPanel;
        DialogController.OnScheduleMenuRequested += EnableScheduleMenuPanel;
        ToggleButton.OnToggleChangeRequested += CheckScheduleMenu;
        DialogSubjectButton.OnDialogSubjectButtonRequested += OnClickDialogSubjectButton;
    }

    private void Start()
    {
        GameManager.Instance.SetGameState(TAG, GameState.UI); // 입력을 UI모드로 변경

        SetDayNight(); // 아침인지 낮인지 구분

        SetStartDialog(); // 아침, 저녁 별 첫 대화 정하기

        SetAvailableDialogSubject(); // 대화 주제 정하기

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
        DialogSubjectButton.OnDialogSubjectButtonRequested -= OnClickDialogSubjectButton;
    }

    private void SetDayNight()
    {
        // todo 아침인지 저녁인지 판별
        int currentYear = TimeManager.Instance.currentYear;
        int currentHour = TimeManager.Instance.currentHour;
        int currentMinute = TimeManager.Instance.currentMinute;
        // Debug.Log($"{TAG} currentHour {currentHour}");

        // 나가기, 잠자기 버튼 변경
        if (currentHour > 19)
        {
            backgroundController.SetNightSprite();
            isDay = false;
            exitAndSleepButton.GetComponentInChildren<TMP_Text>().text = "잠자기";
        }
        else
        {
            backgroundController.SetDaySprite();
            isDay = true;
            exitAndSleepButton.GetComponentInChildren<TMP_Text>().text = "나가기";
        }
    }

    // 선택 가능한 스케줄 리스트 만들기
    private void SetAvailableSchedule()
    {
        List<(ScheduleType, string)> toggleButtonTupleList = new()
        {
            (ScheduleType.EXERCISE, scheduleTextDic[ScheduleType.EXERCISE]),
            (ScheduleType.HOME_WORK, scheduleTextDic[ScheduleType.HOME_WORK]),
            (ScheduleType.PLAY, scheduleTextDic[ScheduleType.PLAY]),
        };

        // todo schedule 어떤게 가능한지 판별하기
        if (true) // 독서 가능할 때
        {
            toggleButtonTupleList.Add((ScheduleType.READ_BOOK, scheduleTextDic[ScheduleType.READ_BOOK]));
        }
        if (true)
        {
            toggleButtonTupleList.Add((ScheduleType.DRAWING, scheduleTextDic[ScheduleType.DRAWING]));
        }
        if (true)
        {
            toggleButtonTupleList.Add((ScheduleType.COOK, scheduleTextDic[ScheduleType.COOK]));
        }
        if (true)
        {
            toggleButtonTupleList.Add((ScheduleType.MUSIC, scheduleTextDic[ScheduleType.MUSIC]));
        }
        if (true)
        {
            toggleButtonTupleList.Add((ScheduleType.DOLL, scheduleTextDic[ScheduleType.DOLL]));
        }

        for (int i = 0; i < toggleButtonTupleList.Count; i++)
        {
            string panelName = i < maxToggleButtonNum / 2 ? "Panel1" : "Panel2";
            ToggleButton tb = Instantiate(toggleButtonPrefab, scheduleMenu.transform.Find(panelName)).GetComponent<ToggleButton>();
            tb.SetText(toggleButtonTupleList[i].Item2);
            tb.SetScheduleType(toggleButtonTupleList[i].Item1);
            toggleButtonList.Add(tb);
        }
    }

    // 선택 가능한 대화 주제 리스트 만들기
    private void SetAvailableDialogSubject()
    {

        List<string> dialogSubejctButtonStringList = new()
        {
            dialogSubjectDic[SituationType.WANT_TO_DO],
            dialogSubjectDic[SituationType.WANT_TO_BE],
            dialogSubjectDic[SituationType.WANT_TO_EAT]
        };

        List<SituationType> situationTypeList = new(){
            SituationType.WANT_TO_DO,
            SituationType.WANT_TO_BE,
            SituationType.WANT_TO_EAT
        };

        for (int i = 0; i < dialogSubejctButtonStringList.Count; i++)
        {
            string panelName = i < maxToggleButtonNum / 2 ? "Panel1" : "Panel2";
            DialogSubjectButton dialogSubjectButton = Instantiate(dialogSubjectButtonPrefab, dialogMenu.transform.Find(panelName)).GetComponent<DialogSubjectButton>();
            dialogSubjectButton.SetText(dialogSubejctButtonStringList[i]);
            dialogSubjectButton.SetSituationType(situationTypeList[i]);
            dialogSubjectButtonList.Add(dialogSubjectButton);
        }
    }

    // 스케줄 토글 버튼 클릭 시 실행, 2개 선택되면 yes/no 창 띄움
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

    // 아침 저녁의 첫 대화 정하기
    private void SetStartDialog()
    {
        int currentDay = TimeManager.Instance.currentDay;

        if (currentDay == 1 && isDay)
        {
            // 게임 처음 시작 시 대화
            npcDialog.currentDialogId = introDialogId;
            return;
        }

        if (isDay)
        {
            List<Dialog> dayDialogList = DialogTool.GetDialogListBySituation(SituationType.MORNING, DaughterManager.Instance.GetStats());
            npcDialog.currentDialogId = dayDialogList[UnityEngine.Random.Range(0, dayDialogList.Count)].id;
        }
        else
        {
            List<Dialog> nightDialogList = DialogTool.GetDialogListBySituation(SituationType.EVENING, DaughterManager.Instance.GetStats());
            npcDialog.currentDialogId = nightDialogList[UnityEngine.Random.Range(0, nightDialogList.Count)].id;
        }
    }

    // 대화 주제 선택 시 대화 진행
    public void OnClickDialogSubjectButton(SituationType situationType)
    {
        // Debug.Log($"{TAG} text {situationType}");

        StaminaManager.Instance.ConsumeStamina();
        DisableDialogMenuPanel();

        // 여기서 대화
        List<Dialog> dialogList = DialogTool.GetDialogListBySituation(situationType, DaughterManager.Instance.GetStats());
        Dialog dialog = dialogList[UnityEngine.Random.Range(0, dialogList.Count)];
        npcDialog.currentDialogId = dialog.id;
        dialogController.InitDialog(npcDialog);
    }

    // 기본 메뉴 On/Off
    public void EnableNormalMenuPanel(bool sw)
    {
        normalMenu.SetActive(sw);
    }

    // 나가기/잠자기 버튼
    public void ExitAndSleepButton(bool sw)
    {
        if (isDay)
        {
            EnableScheduleMenuPanel(sw);
        }
        else
        {
            Debug.Log($"{TAG} 잠자기");
        }
    }

    // 스케쥴 메뉴 On/Off
    public void EnableScheduleMenuPanel(bool sw)
    {
        normalMenu.SetActive(!sw);
        scheduleMenu.SetActive(sw);
    }

    // 대화 주제 선택 메뉴 On/Off
    public void EnableDialogMenuPanel(bool sw)
    {
        normalMenu.SetActive(!sw);
        dialogMenu.SetActive(sw);
    }

    // 대화 주제 선택 메뉴 Off
    public void DisableDialogMenuPanel()
    {
        dialogMenu.SetActive(false);
    }

    // 스케줄 지정 완료
    public void YesExit()
    {
        for (int i = 0; i < toggleButtonList.Count; i++)
        {
            if (toggleButtonList[i].toggle.isOn)
            {
                // Debug.Log($"{TAG} toggleButtonList {i} isOn");
                // Debug.Log($"{TAG} GameManger {GameManager.Instance.wantedScheduleType}");
                // Debug.Log($"{TAG} toggleButtonList {GameManager.Instance.actualScheuleType[0]}");
                GameManager.Instance.actualScheuleType.Add(toggleButtonList[i].scheduleType); // 진행할 스케줄 2개 저장
            }
        }
        OnExitRequested?.Invoke();
    }

    // 스케줄 지정 취소
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
        //npcDialog.currentDialogId = testIndex;

        npcDialog.currentDialogId = dialogTestIndex;
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
