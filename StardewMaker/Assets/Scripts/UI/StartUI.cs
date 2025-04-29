using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartUIManager : MonoBehaviour
{
    [Header("Button Link")]
    [SerializeField] private Button startButton;
    [SerializeField] private Button optionButton;
    [SerializeField] private Button quitButton;

    [Header("Option Popup")]
    [SerializeField] private GameObject optionPopup;
    [SerializeField] private Button cancelButton;

    [Header("Change Secne Name")]
    [SerializeField] private string gameSceneName;

    private void Start()
    {
        if (startButton != null)
        {
            startButton.onClick.AddListener(OnStartButtonClick);
        }
        if (optionButton != null)
        {
            optionButton.onClick.AddListener(OnOptionButtonClick);
        }
        if (quitButton != null)
        {
            quitButton.onClick.AddListener(OnQuitButtonClick);
        }
        if (cancelButton != null)
        {
            cancelButton.onClick.AddListener(OnCancelButtonClick);
        }

        if (optionPopup != null)
        {
            optionPopup.SetActive(false); // 시작할 때는 팝업 꺼두기
        }
    }

    private void OnStartButtonClick()
    {
        SceneManager.LoadScene(gameSceneName);
    }

    private void OnOptionButtonClick()
    {
        if (optionPopup != null)
        {
            optionPopup.SetActive(true);
        }
    }

    private void OnCancelButtonClick()
    {
        if (optionPopup != null)
        {
            optionPopup.SetActive(false); // 팝업 끄기
        }
    }

    private void OnQuitButtonClick()
    {
        Application.Quit();
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false; // 유니티 에디터에서 테스트할 때도 종료
        #endif
    }
}
