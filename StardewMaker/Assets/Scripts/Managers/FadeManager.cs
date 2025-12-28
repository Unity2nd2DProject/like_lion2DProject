using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FadeManager : Singleton<FadeManager>
{
    [SerializeField] private Image fadeImage;
    [SerializeField] public float fadeDuration;
    public bool IsFading { get; private set; } // 추가

    protected override void Awake()
    {
        base.Awake();
    }

    public void FadeIn(System.Action onComplete = null)
    {
        StartCoroutine(Fade(1f, 0f, onComplete));
    }

    public void FadeOut(System.Action onComplete = null)
    {
        StartCoroutine(Fade(0f, 1f, onComplete));
    }

    private IEnumerator Fade(float startAlpha, float endAlpha, System.Action onComplete)
    {
        IsFading = true; // 페이드 시작
        fadeImage.raycastTarget = true; // 시작 시 차단

        float time = 0f;
        Color color = fadeImage.color;
        color.a = startAlpha;
        fadeImage.color = color;

        while (time < fadeDuration)
        {
            time += Time.deltaTime;
            float alpha = Mathf.Lerp(startAlpha, endAlpha, time / fadeDuration);
            color.a = alpha;
            fadeImage.color = color;
            yield return null;
        }

        // 보정
        color.a = endAlpha;
        fadeImage.color = color;

        fadeImage.raycastTarget = false; // 끝나면 클릭 허용
        IsFading = false; // 페이드 종료
        onComplete?.Invoke();
    }
}
