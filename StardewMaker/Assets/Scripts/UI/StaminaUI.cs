using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public enum StaminaState
{
    Full,
    Half,
    Empty
}

public class StaminaUI : Singleton<StaminaUI>
{
    [Header("Stamina Settings")]
    public Image[] staminaIcons;
    public Sprite fullSprite;
    public Sprite halfSprite;
    public Sprite emptySprite;

    protected override void Awake()
    {
        base.Awake();
    }

    public void InitializeUI(int staminaCount)
    {
        // 스태미나 아이콘 초기화
        for (int i = 0; i < staminaIcons.Length; i++)
        {
            if (i < staminaCount * 2) // 반칸 단위로 계산 (Full = 2 Half)
            {
                staminaIcons[i].gameObject.SetActive(true);
                staminaIcons[i].sprite = fullSprite;
            }
            else
            {
                staminaIcons[i].gameObject.SetActive(false);
            }
        }
    }

    public void UpdateStaminaUI(StaminaState[] states)
    {
        for (int i = 0; i < staminaIcons.Length; i++)
        {
            switch (states[i])
            {
                case StaminaState.Full:
                    staminaIcons[i].sprite = fullSprite;
                    break;
                case StaminaState.Half:
                    staminaIcons[i].sprite = halfSprite;
                    break;
                case StaminaState.Empty:
                    staminaIcons[i].sprite = emptySprite;
                    break;
            }
        }
    }

    public void ShakeUI(float duration = 0.2f, float magnitude = 2f)
    {
        StartCoroutine(ShakeCoroutine(duration, magnitude));
    }

    private IEnumerator ShakeCoroutine(float duration, float magnitude)
    {
        RectTransform rt = gameObject.GetComponent<RectTransform>();
        Vector3 originalPos = rt.anchoredPosition;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float offsetX = Random.Range(-2f, 2f) * magnitude;
            float offsetY = Random.Range(-2f, 2f) * magnitude;

            rt.anchoredPosition = originalPos + new Vector3(offsetX, offsetY, 0);
            elapsed += Time.deltaTime;
            yield return null;
        }

        rt.anchoredPosition = originalPos;
    }
}