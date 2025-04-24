using System.Collections;
using UnityEngine;

public class PrincessScene1Controller : MonoBehaviour
{
    private string TAG = "[PrincessScene1Controller]";
    private UserInputManager inputManager;

    [SerializeField] private DialogController dialogController;
    [SerializeField] private GameObject buttonPanel;
    private NPCDialog npcDialog;

    // todo initialDialogId는 DB에서 가져와야 함

    void Awake()
    {
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

        StartCoroutine(StartScene1()); // 씬1 시작
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
        // todo 페이드인
        yield return new WaitForSeconds(1);

        // 대화 시작
        dialogController.InitDialog(npcDialog);
    }

}
