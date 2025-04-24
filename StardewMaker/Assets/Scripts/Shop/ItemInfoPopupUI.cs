using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemInfoPopupUI : MonoBehaviour
{
    public static ItemInfoPopupUI Instance { get; private set; }

    [Header("UI Components")]
    public Image icon;
    public TextMeshProUGUI itemNameText;
    public TextMeshProUGUI descriptionText;

    [Header("Settings")]
    public float followSpeed = 50f; // 마우스를 따라가는 속도

    private RectTransform rectT;
    private Canvas canvas;
    private CanvasGroup canvasGroup;

    private bool isShowing = false;

    void Awake()
    {
        // 싱글톤 할당
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        rectT = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();

        // CanvasGroup 설정
        if (!TryGetComponent(out canvasGroup))
            canvasGroup = gameObject.AddComponent<CanvasGroup>();

        canvasGroup.blocksRaycasts = false;

        Hide();
    }

    // 아이템 정보 표시
    public void Show(ItemData item, Vector2 screenPosition)
    {
        if (item == null)
        {
            Hide();
            return;
        }

        icon.sprite = item.icon;
        itemNameText.text = item.itemName;
        descriptionText.text = item.itemDescription;

        gameObject.SetActive(true);
        isShowing = true;

        Follow(screenPosition);
    }
    
    // 마우스 따라다니게 하기
    public void Follow(Vector2 screenPosition)
    {
        if (!isShowing) return;

        // 스크린 좌표를 캔버스 로컬 좌표로 변환
        Vector2 localPos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.transform as RectTransform,
            screenPosition,
            canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : canvas.worldCamera,
            out localPos);

        // 팝업 크기 계산
        Vector2 popupSize = rectT.sizeDelta * canvas.scaleFactor;

        // 마우스 커서에서 약간 떨어진 오프셋
        Vector2 offset = new Vector2(popupSize.x / 2f + 10f, -popupSize.y / 2f - 10f);
        Vector2 target = localPos + offset;

        // 클램프 처리 (캔버스 영역 안으로 제한)
        float halfW = canvas.pixelRect.width / 2f;
        float halfH = canvas.pixelRect.height / 2f;
        float clampedX = Mathf.Clamp(target.x, -halfW + popupSize.x / 2f, halfW - popupSize.x / 2f);
        float clampedY = Mathf.Clamp(target.y, -halfH + popupSize.y / 2f, halfH - popupSize.y / 2f);

        rectT.anchoredPosition = new Vector2(clampedX, clampedY);
    }

    // 팝업 숨기기
    public void Hide()
    {
        gameObject.SetActive(false);
        isShowing = false;
    }
}