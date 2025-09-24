using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightManager : MonoBehaviour
{
    public static LightManager Instance;

    [Header("Light Settings")]
    public Light2D directionalLight;

    [Header("Light Colors")]
    public Color morningColor = new Color(1f, 1f, 1f, 1f);
    public Color sunsetColor = new Color(1f, 0.7f, 0.5f, 1f);
    public Color nightColor = new Color(0.2f, 0.3f, 0.6f, 1f);

    // 날씨 효과를 위한 필드 추가
    [HideInInspector] public float weatherMultiplier = 1.0f;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }


    public void UpdateLighting(int hour, int minute)
    {
        float currentTime = hour + (minute / 60f);
        Color targetColor = morningColor;

        if (currentTime >= 16f && currentTime < 18f)
        {
            float t = (currentTime - 16f) / 2f;
            targetColor = Color.Lerp(morningColor, sunsetColor, t);
        }
        else if (currentTime >= 18f && currentTime < 20f)
        {
            float t = (currentTime - 18f) / 2f;
            targetColor = Color.Lerp(sunsetColor, nightColor, t);
        }
        else if (currentTime >= 20f && currentTime < 24f)
        {
            targetColor = nightColor;
        }
        else if (currentTime >= 0f && currentTime < 7f)
        {
            float t = currentTime / 7f;
            targetColor = Color.Lerp(nightColor, morningColor, t);
        }

        // 날씨 효과 적용 - 여기서 weatherMultiplier 사용
        targetColor = new Color(
            targetColor.r * weatherMultiplier,
            targetColor.g * weatherMultiplier,
            targetColor.b * weatherMultiplier,
            targetColor.a
        );

        directionalLight.color = targetColor;
    }
}
