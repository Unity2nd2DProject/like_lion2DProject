using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ItemAddEffect : MonoBehaviour
{
    public float moveDistance = 50f; // 위로 올라갈 거리
    public float duration = 0.8f;    // 애니메이션 시간

    private RectTransform rectTransform;
    private Image image;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        image = GetComponent<Image>();
    }

    public void Play(ItemData itemData, Vector3 startScreenPosition)
    {
        Debug.Log("ItemAddEffect Play");
        image.sprite = itemData.icon;
        rectTransform.position = startScreenPosition;
        StartCoroutine(MoveAndFade());
    }

    private IEnumerator MoveAndFade()
    {
        Vector3 startPos = rectTransform.position;
        Vector3 endPos = startPos + Vector3.up * moveDistance;

        float elapsed = 0f;
        Color startColor = image.color;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;

            // 위치 이동
            rectTransform.position = Vector3.Lerp(startPos, endPos, t);
            // 점점 투명하게
            image.color = new Color(startColor.r, startColor.g, startColor.b, 1f - t);

            yield return null;
        }

        Destroy(gameObject);
    }
}
