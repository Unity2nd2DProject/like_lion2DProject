using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TransparentDetection : MonoBehaviour
{
    [Range(0, 1)]
    [SerializeField] private float transparencyAmount = 0.8f;
    [SerializeField] private float fadeTime = 0.4f;

    private SpriteRenderer sr;
    private Tilemap tm;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        tm = GetComponent<Tilemap>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<PlayerController>())
        {
            if (sr)
            {
                StartCoroutine(FadeRoutine(sr, fadeTime, sr.color.a, transparencyAmount));
            }
            else if (tm)
            {
                StartCoroutine(FadeRoutine(tm, fadeTime, tm.color.a, transparencyAmount));
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<PlayerController>())
        {
            if (sr)
            {
                StartCoroutine(FadeRoutine(sr, fadeTime, sr.color.a, 1f));
            }
            else if (tm)
            {
                StartCoroutine(FadeRoutine(tm, fadeTime, tm.color.a, 1f));
            }
        }

    }

    private IEnumerator FadeRoutine(SpriteRenderer sr, float fadeTime, float startValue, float targetTransparency)
    {
        float elapsedTime = 0;
        while (elapsedTime < fadeTime)
        {
            elapsedTime += Time.deltaTime;
            float newAlpha = Mathf.Lerp(startValue, targetTransparency, elapsedTime / fadeTime);
            sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, newAlpha);
            yield return null;
        }
    }

    private IEnumerator FadeRoutine(Tilemap tm, float fadeTime, float startValue, float targetTransparency)
    {
        float elapsedTime = 0;
        while (elapsedTime < fadeTime)
        {
            elapsedTime += Time.deltaTime;
            float newAlpha = Mathf.Lerp(startValue, targetTransparency, elapsedTime / fadeTime);
            tm.color = new Color(tm.color.r, tm.color.g, tm.color.b, newAlpha);
            yield return null;
        }
    }
}
