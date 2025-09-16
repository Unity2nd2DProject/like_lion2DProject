using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PrincessScene1Controller : MonoBehaviour
{
    private string TAG = "[PrincessScene1Controller]";
    private UserInputManager inputManager; // 사용자 입력. 여기서 안 씀
    public static event Action OnExitRequested; // 홈에서 나가기

    [Header("GameObject들 관리")]
    [SerializeField] private DialogController dialogController;
    [SerializeField] private GameObject normalMenu;
    [SerializeField] private GameObject scheduleMenu;
    [SerializeField] private GameObject dialogMenu;
    [SerializeField] private GameObject yesNoMenu;
    [SerializeField] private GameObject yesNoImage;
    [SerializeField] private GameObject exitAndSleepButton;
    [SerializeField] private TMP_Text statText;
    [SerializeField] private GameObject statChangePopup;

    [Space]
    [SerializeField] private GameObject toggleButtonPrefab;
    [SerializeField] private GameObject dialogSubjectButtonPrefab;
    [SerializeField] private int dialogTestIndex;

    private NPCDialog npcDialog;
    private BackgroundController backgroundController;

    private List<ToggleButton> toggleButtonList;
    private int maxToggleButtonNum = 8;
    private bool isDay = true;

    private List<DialogSubjectButton> dialogSubjectButtonList;

    private int introDialogId = 2;

    // Stat증감 표현을 위함
    private Queue<string> statChangeQueue = new Queue<string>();
    private bool isShowing = false;

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
        {SituationType.MEMORY, "추억"},
        {SituationType.SWEET, "다정한"},
        {SituationType.TODAY, "오늘하루"},
    };

    void Awake()
    {
        GameManager.Instance.arrivalPointName = "InsideHomeFrontDoor"; //  FadeIn 실행하기 위함

        npcDialog = GetComponent<NPCDialog>();
        backgroundController = GetComponent<BackgroundController>();
    }

    private void OnEnable()
    {
        inputManager = UserInputManager.Instance; // 사용자 입력 받는 용도
        DialogController.OnNormalMenuRequested += EnableNormalMenuPanel;
        DialogController.OnScheduleMenuRequested += EnableScheduleMenuPanel;
        ToggleButton.OnToggleChangeRequested += CheckScheduleMenu;
        DialogSubjectButton.OnDialogSubjectButtonRequested += OnClickDialogSubjectButton;
        Dialog.OnStatChangeRequested += StatChanged;
        DialogController.OnNextDayRequested += CheckAtStart;
        UIManager.OnNormalMenuRequested += EnableNormalMenuPanel;
        GiftManager.OnDialogRequested += OnDialogStart;
        DaughterManager.OnStatChangeRequested += StatChanged;
    }

    private void Start()
    {
        GameManager.Instance.SetGameState(TAG, GameState.UI); // 입력을 UI모드로 변경

        CheckAtStart();
    }

    private void CheckAtStart()
    {
        SetDayNight(); // 아침인지 낮인지 구분

        StatChangeAtNight(); // 저녁에 Stat 증감

        SetStartDialog(); // 아침, 저녁 별 첫 대화 정하기

        SetAvailableDialogSubject(); // 대화 주제 정하기

        SetAvailableSchedule(); // 스케줄 정하기

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
        Dialog.OnStatChangeRequested -= StatChanged;
        DialogController.OnNextDayRequested -= CheckAtStart;
        UIManager.OnNormalMenuRequested -= EnableNormalMenuPanel;
        GiftManager.OnDialogRequested -= OnDialogStart;
        DaughterManager.OnStatChangeRequested -= StatChanged;
    }

    private void SetDayNight()
    {
        int currentYear = TimeManager.Instance.currentYear;
        int currentHour = TimeManager.Instance.currentHour;
        int currentMinute = TimeManager.Instance.currentMinute;
        // Debug.Log($"{TAG} currentHour {currentHour}");

        // 나가기, 잠자기 버튼 변경
        if (currentHour > 17)
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

    private void StatChangeAtNight()
    {
        if (!isDay)
        {
            for (int i = 0; i < GameManager.Instance.actualScheuleType.Count; i++)
            {
                ScheduleType actualScheduleType = GameManager.Instance.actualScheuleType[i];
                switch (actualScheduleType)
                {
                    case ScheduleType.EXERCISE:
                        DaughterManager.Instance.AddStats(StatType.VITALITY, 1);
                        DaughterManager.Instance.AddStats(StatType.PYSICAL, 1);
                        DaughterManager.Instance.AddStats(StatType.HUNGER, -1);
                        break;
                    case ScheduleType.HOME_WORK:
                        DaughterManager.Instance.AddStats(StatType.VITALITY, 1);
                        DaughterManager.Instance.AddStats(StatType.DOMESTIC, 1);
                        break;
                    case ScheduleType.PLAY:
                        DaughterManager.Instance.AddStats(StatType.VITALITY, 1);
                        DaughterManager.Instance.AddStats(StatType.HUNGER, -1);
                        break;
                    case ScheduleType.READ_BOOK:
                        DaughterManager.Instance.AddStats(StatType.ACADEMIC, 1);
                        DaughterManager.Instance.AddStats(StatType.HUNGER, -1);
                        break;
                    case ScheduleType.DRAWING:
                        DaughterManager.Instance.AddStats(StatType.ART, -1);
                        DaughterManager.Instance.AddStats(StatType.HUNGER, -1);
                        break;
                    case ScheduleType.COOK:
                        DaughterManager.Instance.AddStats(StatType.PYSICAL, 1);
                        DaughterManager.Instance.AddStats(StatType.DOMESTIC, 1);
                        DaughterManager.Instance.AddStats(StatType.HUNGER, -1);
                        break;
                    case ScheduleType.MUSIC:
                        DaughterManager.Instance.AddStats(StatType.MUSIC, 1);
                        DaughterManager.Instance.AddStats(StatType.MOOD, 1);
                        DaughterManager.Instance.AddStats(StatType.HUNGER, -1);
                        break;
                    case ScheduleType.DOLL:
                        DaughterManager.Instance.AddStats(StatType.SOCIAL, 1);
                        DaughterManager.Instance.AddStats(StatType.DOMESTIC, 1);
                        DaughterManager.Instance.AddStats(StatType.HUNGER, -1);
                        break;
                }
            }
        }
    }

    // 아침 저녁의 첫 대화 정하기
    private void SetStartDialog()
    {
        int currentDay = TimeManager.Instance.currentDay;
        Season currentSeason = TimeManager.Instance.currentSeason;

        if (currentDay == 1 && currentSeason == Season.Spring && isDay)
        {
            // 게임 처음 시작 시 대화
            // npcDialog.currentDialogId = introDialogId;
            List<Dialog> dayDialogList = DialogTool.GetDialogListBySituation(SituationType.INTRO, DaughterManager.Instance.GetStats());
            npcDialog.currentDialogId = dayDialogList[UnityEngine.Random.Range(0, dayDialogList.Count)].id;
            return;
        }

        if (TimeManager.Instance.IsLastDay())
        {
            List<Dialog> dayDialogList = DialogTool.GetDialogListBySituation(SituationType.ENDING, DaughterManager.Instance.GetStats());
            npcDialog.currentDialogId = dayDialogList[UnityEngine.Random.Range(0, dayDialogList.Count)].id;
            return;
        }

        if (isDay)
        {
            List<Dialog> dayDialogList = DialogTool.GetDialogListBySituation(SituationType.MORNING, DaughterManager.Instance.GetStats());
            npcDialog.currentDialogId = dayDialogList[UnityEngine.Random.Range(0, dayDialogList.Count)].id;
        }
        else
        {
            bool isWantedSchedule = false;
            if (GameManager.Instance.wantedDialog != null)
            {
                ScheduleType wantedScheduleType = GameManager.Instance.wantedDialog.scheduleType;
                List<ScheduleType> actualScheuleType = GameManager.Instance.actualScheuleType;

                foreach (var i in actualScheuleType)
                {
                    if (i == wantedScheduleType)
                    {
                        isWantedSchedule = true;
                        break;
                    }
                }
            }

            if (isWantedSchedule) // 원하는 스케쥴이었을 경우 대사 진행 및 스탯 증감
            {
                Debug.Log($"{TAG} isWantedSchedule");

                // GameManager.Instance.wantedDialog.SetTarget(DaughterManager.Instance.GetStats()); // 여기서 안 하고 대사 끝나면 올리기
                npcDialog.currentDialogId = GameManager.Instance.wantedDialog.id + 1;
                GameManager.Instance.wantedDialog = null;
            }
            else
            {
                List<Dialog> nightDialogList = DialogTool.GetDialogListBySituation(SituationType.EVENING, DaughterManager.Instance.GetStats());
                npcDialog.currentDialogId = nightDialogList[UnityEngine.Random.Range(0, nightDialogList.Count)].id;
            }
        }
    }

    // 선택 가능한 대화 주제 리스트 만들기
    private void SetAvailableDialogSubject()
    {
        dialogSubjectButtonList = new();

        List<string> dialogSubejctButtonStringList = isDay ? new()
        {
            dialogSubjectDic[SituationType.WANT_TO_DO],
            // dialogSubjectDic[SituationType.WANT_TO_BE],
            // dialogSubjectDic[SituationType.WANT_TO_EAT],
            dialogSubjectDic[SituationType.MEMORY],
            dialogSubjectDic[SituationType.SWEET],
            // dialogSubjectDic[SituationType.TODAY],
        } : new()
        {
            // dialogSubjectDic[SituationType.WANT_TO_DO],
            dialogSubjectDic[SituationType.WANT_TO_BE],
            // dialogSubjectDic[SituationType.WANT_TO_EAT],
            dialogSubjectDic[SituationType.MEMORY],
            // dialogSubjectDic[SituationType.SWEET],
            dialogSubjectDic[SituationType.TODAY],
        };

        List<SituationType> situationTypeList = isDay ? new(){
            SituationType.WANT_TO_DO,
            // SituationType.WANT_TO_BE,
            // SituationType.WANT_TO_EAT,
            SituationType.MEMORY,
            SituationType.SWEET,
            // SituationType.TODAY
        } : new(){
            // SituationType.WANT_TO_DO,
            SituationType.WANT_TO_BE,
            // SituationType.WANT_TO_EAT,
            SituationType.MEMORY,
            // SituationType.SWEET,
            SituationType.TODAY
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

    // 선택 가능한 스케줄 리스트 만들기
    private void SetAvailableSchedule()
    {
        toggleButtonList = new();

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

    public void ResetScheduleDialogMenu()
    {
        foreach (Transform child in scheduleMenu.transform.Find("Panel1"))
        {
            Destroy(child.gameObject);
        }
        foreach (Transform child in scheduleMenu.transform.Find("Panel2"))
        {
            Destroy(child.gameObject);
        }
        foreach (Transform child in dialogMenu.transform.Find("Panel1"))
        {
            Destroy(child.gameObject);
        }
        foreach (Transform child in dialogMenu.transform.Find("Panel2"))
        {
            Destroy(child.gameObject);
        }
    }

    // 기본 메뉴 On/Off
    public void EnableNormalMenuPanel(bool sw)
    {
        normalMenu.SetActive(sw);
    }

    // 스케쥴 메뉴 On/Off
    public void EnableScheduleMenuPanel(bool sw)
    {
        normalMenu.SetActive(!sw);
        scheduleMenu.SetActive(sw);
    }

    // 나가기/잠자기 버튼
    public void ExitAndSleepButton(bool sw)
    {
        if (isDay)
        {
            EnableScheduleMenuPanel(sw);
        }
        else // 잠자기
        {
            List<Dialog> dialogList = DialogTool.GetDialogListBySituation(SituationType.SLEEP, DaughterManager.Instance.GetStats());
            Dialog dialog = dialogList[UnityEngine.Random.Range(0, dialogList.Count)];
            npcDialog.currentDialogId = dialog.id;
            dialogController.InitDialog(npcDialog);

            ResetScheduleDialogMenu(); // 게임오브젝트 삭제
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

    // 대화 주제 선택 시 대화 진행
    public void OnClickDialogSubjectButton(SituationType situationType)
    {
        // Debug.Log($"{TAG} text {situationType}");

        if (!StaminaManager.Instance.CanConsumeStamina())
        {
            return;
        }
        StaminaManager.Instance.ConsumeStamina();
        DisableDialogMenuPanel();

        // 여기서 대화
        List<Dialog> dialogList = DialogTool.GetDialogListBySituation(situationType, DaughterManager.Instance.GetStats());
        Dialog dialog = dialogList[UnityEngine.Random.Range(0, dialogList.Count)];
        npcDialog.currentDialogId = dialog.id;
        dialogController.InitDialog(npcDialog);
    }

    // Stat 변경 시 화면에 표시
    private void StatChanged(string str)
    {
        // statText.text = str;
        // StartCoroutine(ShowStatChangePopup());

        statChangeQueue.Enqueue(str);
        if (!isShowing)
        {
            StartCoroutine(StatChangePopupQueue());
        }
    }


    private IEnumerator StatChangePopupQueue()
    {
        isShowing = true;

        while (statChangeQueue.Count > 0)
        {
            string str = statChangeQueue.Dequeue();
            statText.text = str;
            yield return ShowStatChangePopup();
        }

        isShowing = false;
    }

    private IEnumerator ShowStatChangePopup()
    {
        RectTransform rect = statChangePopup.GetComponent<RectTransform>();
        Vector3 originalPos = rect.anchoredPosition;

        statChangePopup.SetActive(true);
        rect.anchoredPosition = originalPos;

        SoundManager.Instance.PlaySFX("StatPopup");

        CanvasGroup cg = statChangePopup.GetComponent<CanvasGroup>();
        if (cg == null)
            cg = statChangePopup.AddComponent<CanvasGroup>();

        // 초기 상태
        cg.alpha = 0f;

        // fade-in
        float duration = 0.2f;
        for (float t = 0; t < duration; t += Time.deltaTime)
        {
            cg.alpha = Mathf.Lerp(0f, 1f, t / duration);
            yield return null;
        }
        cg.alpha = 1f;

        // 대기 시간
        yield return new WaitForSeconds(0.5f);

        // 위로 올라가면서 fade-out
        Vector3 targetPos = originalPos + new Vector3(0, 50f, 0);
        duration = 0.5f;
        for (float t = 0; t < duration; t += Time.deltaTime)
        {
            cg.alpha = Mathf.Lerp(1f, 0f, t / duration);
            rect.anchoredPosition = Vector3.Lerp(originalPos, targetPos, t / duration);
            yield return null;
        }
        cg.alpha = 0f;
        rect.anchoredPosition = originalPos;
        statChangePopup.SetActive(false);
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

    IEnumerator StartScene1()
    {
        // ArrivalPoint에서 FadeIn 진행
        yield return new WaitForSeconds(FadeManager.Instance.fadeDuration);

        // 대화 시작
        dialogController.InitDialog(npcDialog);
    }

    private void MoveInput()
    {
        Vector2 moveInput = inputManager.inputActions.UI.Move.ReadValue<Vector2>();
    }

    public void DialogTest()
    {
        if (!StaminaManager.Instance.CanConsumeStamina())
        {
            return;
        }
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


    public void OnClickCookButton()
    {
        List<Dialog> cookDialogList = DialogTool.GetDialogListBySituation(SituationType.COOK, DaughterManager.Instance.GetStats());
        npcDialog.currentDialogId = cookDialogList[UnityEngine.Random.Range(0, cookDialogList.Count)].id;
        dialogController.InitDialog(npcDialog);
        // UIManager.Instance.ToggleCookingUI();
    }

    public void OnClickGiftButton()
    {
        List<Dialog> cookDialogList = DialogTool.GetDialogListBySituation(SituationType.GIFT, DaughterManager.Instance.GetStats());
        npcDialog.currentDialogId = cookDialogList[UnityEngine.Random.Range(0, cookDialogList.Count)].id;
        dialogController.InitDialog(npcDialog);
        // UIManager.Instance.ToggleGiftUI();
    }

    public void OnDialogStart(SituationType situationType)
    {
        List<Dialog> dialogList = DialogTool.GetDialogListBySituation(situationType, DaughterManager.Instance.GetStats());
        npcDialog.currentDialogId = dialogList[UnityEngine.Random.Range(0, dialogList.Count)].id;
        dialogController.InitDialog(npcDialog);
    }
}
