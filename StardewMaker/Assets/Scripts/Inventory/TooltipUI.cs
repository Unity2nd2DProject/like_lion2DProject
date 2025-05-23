using TMPro;
using UnityEngine;

public class TooltipUI : MonoBehaviour
{
    CanvasGroup canvasGroup;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI descriptionText;

    public void ShowTooltip(ItemData item, Vector3 mousePosition)
    {
        if (canvasGroup != null)
        {
            canvasGroup.blocksRaycasts = false;
        }
        gameObject.transform.SetParent(GameObject.FindGameObjectWithTag("MainCanvas").transform);
        gameObject.transform.SetAsLastSibling(); // Ensure the tooltip is on top of other UI elements
        gameObject.SetActive(true);

        nameText.text = item.itemName;
        descriptionText.text = item.itemDescription;

        RectTransform canvasRect = gameObject.GetComponentInParent<Canvas>().GetComponent<RectTransform>();
        RectTransform tooltipRect = gameObject.GetComponent<RectTransform>();

        Vector2 anchoredPos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, mousePosition, null, out anchoredPos);

        Vector2 offset = new Vector2(20f, -20f);
        anchoredPos += offset;

        float tooltipWidth = tooltipRect.rect.width;
        float tooltipHeight = tooltipRect.rect.height;
        float canvasWidth = canvasRect.rect.width;
        float canvasHeight = canvasRect.rect.height;
        float margin = 20f;

        // pivt (0.1, 0.9)
        float pivotOffsetX = tooltipWidth * 0.1f;
        float pivotOffsetY = tooltipHeight * 0.9f;

        // right
        float rightEdge = anchoredPos.x + (tooltipWidth - pivotOffsetX);
        float rightOverflow = rightEdge - (canvasWidth / 2 - margin);
        if (rightOverflow > 0)
        {
            anchoredPos.x -= rightOverflow;
        }

        // left
        float leftEdge = anchoredPos.x - pivotOffsetX;
        float leftOverflow = (-canvasWidth / 2 + margin) - leftEdge;
        if (leftOverflow > 0)
        {
            anchoredPos.x += leftOverflow;
        }

        // top
        float topEdge = anchoredPos.y + (tooltipHeight - pivotOffsetY);
        float topOverflow = topEdge - (canvasHeight / 2 - margin);
        if (topOverflow > 0)
        {
            anchoredPos.y -= topOverflow;
        }

        // bottom
        float bottomEdge = anchoredPos.y - pivotOffsetY;
        float bottomOverflow = (-canvasHeight / 2 + margin) - bottomEdge;
        if (bottomOverflow > 0)
        {
            anchoredPos.y += bottomOverflow;
        }

        tooltipRect.anchoredPosition = anchoredPos;

        tooltipRect.anchoredPosition = anchoredPos;
    }


    public void HideTooltip()
    {
        gameObject.SetActive(false);
    }
}
