using UnityEngine;

public class CloudMover : MonoBehaviour
{
    public float speed = 0.5f;
    public float destroyOffset = -15f;
    public float fadeInTime = 1f;
    public float fadeOutTime = 1f;

    private enum FadeState { FadeIn, Normal, FadeOut }
    private FadeState currentState = FadeState.FadeIn;
    private float fadeTimer = 0f;
    private SpriteRenderer spriteRenderer;
    private float targetAlpha;

    void Start()
    {
        // 스프라이트 렌더러와 초기 알파값 설정
        spriteRenderer = GetComponent<SpriteRenderer>();
        targetAlpha = spriteRenderer.color.a; // 기존 오브젝트의 알파값 저장

        // 페이드 인을 위해 초기 알파값을 0으로 설정
        Color startColor = spriteRenderer.color;

    }

    void Update()
    {
        // 이동 로직
        transform.Translate(Vector3.left * speed * Time.deltaTime);

        // 페이드 상태에 따른 처리
        switch (currentState)
        {
            case FadeState.FadeIn:
                HandleFadeIn();
                break;
            case FadeState.Normal:
                CheckFadeOutCondition();
                break;
            case FadeState.FadeOut:
                HandleFadeOut();
                break;
        }
    }

    void HandleFadeIn()
    {
        fadeTimer += Time.deltaTime;
        float progress = fadeTimer / fadeInTime;
        float currentAlpha = Mathf.Lerp(0f, targetAlpha, progress);

        Color color = spriteRenderer.color;
        spriteRenderer.color = new Color(color.r, color.g, color.b, currentAlpha);

        if (progress >= 1f)
        {
            currentState = FadeState.Normal;
            fadeTimer = 0f;
        }
    }

    void CheckFadeOutCondition()
    {
        float destroyX = Camera.main.transform.position.x + destroyOffset;
        if (transform.position.x < destroyX)
        {
            currentState = FadeState.FadeOut;
            fadeTimer = 0f;
        }
    }

    void HandleFadeOut()
    {
        fadeTimer += Time.deltaTime;
        float progress = fadeTimer / fadeOutTime;
        float currentAlpha = Mathf.Lerp(targetAlpha, 0f, progress);

        Color color = spriteRenderer.color;
        spriteRenderer.color = new Color(color.r, color.g, color.b, currentAlpha);

        if (progress >= 1f)
        {
            Destroy(gameObject);
        }
    }
}
