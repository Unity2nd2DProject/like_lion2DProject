using UnityEngine;
using UnityEngine.UI;

public class ScrollResetter : MonoBehaviour
{
    public ScrollRect scrollRect;
    public Scrollbar scrollbar;

    [Range(0f, 1f)]
    public float fixedHandleSize = 0.1f; // 고정할 핸들 크기

    void LateUpdate()
    {
        if (scrollbar != null)
        {
            scrollbar.size = fixedHandleSize;
        }
    }

    void OnEnable()
    {
        // 다음 프레임에서 스크롤 위치 초기화
        StartCoroutine(ResetScrollPosition());
    }

    private System.Collections.IEnumerator ResetScrollPosition()
    {
        yield return null; // 한 프레임 기다림
        scrollRect.verticalNormalizedPosition = 1f; // 1 = 가장 위
    }
}
