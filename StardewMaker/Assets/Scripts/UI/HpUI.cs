using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public enum HpState
{
    Full,
    Half,
    Empty
}

public class HpUI : Singleton<HpUI>
{
    [Header("Stamina Settings")]
    public Image[] hearts;
    public Sprite fullSprite;
    public Sprite halfSprite;
    public Sprite emptySprite;

    protected override void Awake()
    {
        base.Awake();
    }

    public void InitializeUI(int staminaCount)
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            if (i < staminaCount * 2)
            {
                hearts[i].gameObject.SetActive(true);
                hearts[i].sprite = fullSprite;
            }
            else
            {
                hearts[i].gameObject.SetActive(false);
            }
        }
    }

    public void UpdateHearts(int currentHP)
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            int heartValue = currentHP - (i * 10);

            if (heartValue >= 10)
            {
                hearts[i].sprite = fullSprite;
            }
            else if (heartValue >= 5)
            {
                hearts[i].sprite = halfSprite;
            }
            else
            {
                hearts[i].sprite = emptySprite;
            }
        }

        if (currentHP <= 10)
        {
            ShakeUI();
            PlayerController.Instance.TeleportToTown();
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