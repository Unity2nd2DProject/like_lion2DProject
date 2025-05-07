using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogManager : MonoBehaviour
{
    [SerializeField] private GameObject dialogUI;
    [SerializeField] private GameObject shopUI;
    [SerializeField] private GameObject buttonPanel;

    [SerializeField] private TypewriterEffect typewriterEffect;
    [SerializeField] private TextMeshProUGUI dialogueText;

    private bool hasShownButtons = false;

    void Start()
    {
        if (dialogUI != null)
        {
            dialogUI.SetActive(false); 
        }
        if (shopUI != null)
        {
            shopUI.SetActive(false); 
        }

        hasShownButtons = false;

        if (buttonPanel != null)
        {
            buttonPanel.SetActive(false); 
        }
    }

    void Update()
    {
        // TypewriterEffect 내부에서 텍스트가 완전히 출력됐는지 감지
        if (!hasShownButtons &&
            typewriterEffect != null &&
            dialogueText != null &&
            dialogueText.text == typewriterEffect.fullText)
        {
            ShowButtons();
            SoundManager.Instance.StopSfx();
        }
    }

    private void ShowButtons()
    {
        if (buttonPanel != null)
        {
            buttonPanel.SetActive(true);
            hasShownButtons = true;
        }
    }

    // NPC 대화 시작 시 호출
    public void StartDialog()
    {
        if (dialogUI != null)
        {
            dialogUI.SetActive(true);
        }

        if (buttonPanel != null)
        {
            buttonPanel.SetActive(false);
        }

        hasShownButtons = false;

        if (typewriterEffect != null)
        {
            typewriterEffect.StartTyping(); // 텍스트 다시 시작
        }
    }

    public void OnClickBuy()
    {
        if (shopUI != null)
        {
            shopUI.transform.SetAsFirstSibling();
            shopUI.SetActive(true);
            UIManager.Instance.UpdateInventoryUI();
            UIManager.Instance.inventoryUI.gameObject.SetActive(true);
            Time.timeScale = 0f;
        }

        OnClickExit();
    }

    public void OnClickExit()
    {
        // 다이얼로그 UI 닫기
        if (dialogUI != null)
        {
            dialogUI.SetActive(false);
        }

        // 타이핑 텍스트 초기화 (null로 설정)
        if (typewriterEffect != null)
        {
            // 텍스트 초기화
            dialogueText.text = "";
        }

        // 버튼 패널 초기화
        if (buttonPanel != null)
        {
            buttonPanel.SetActive(false); // 버튼 패널 숨기기
        }

        // 버튼 표시가 이미 안 되도록 상태 초기화
        hasShownButtons = false;
    }
}