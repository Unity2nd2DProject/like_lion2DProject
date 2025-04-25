using UnityEngine;
using UnityEngine.UI;

public class ScrollResetter : MonoBehaviour
{
    public ScrollRect scrollRect;

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
