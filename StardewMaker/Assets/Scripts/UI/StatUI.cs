using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static System.Net.Mime.MediaTypeNames;

public class StatUI : MonoBehaviour
{
    public GameObject toggleButton;
    public GameObject endingStatTexts;

    [Header("Common Stat Rows")]
    [SerializeField] private StatRowUI moodRow;
    [SerializeField] private StatRowUI vitalityRow;
    [SerializeField] private StatRowUI hungerRow;
    [SerializeField] private StatRowUI trustRow;

    [Header("Ending Stat Rows")]
    [SerializeField] private StatRowUI physicalRow;
    [SerializeField] private StatRowUI musicRow;
    [SerializeField] private StatRowUI artRow;
    [SerializeField] private StatRowUI socialRow;
    [SerializeField] private StatRowUI academicRow;
    [SerializeField] private StatRowUI domesticRow;

    private Dictionary<StatType, StatRowUI> rowDict;

    bool isExpanded = false;

    private void Awake()
    {
        rowDict = new Dictionary<StatType, StatRowUI>
        {
            { StatType.MOOD, moodRow },
            { StatType.VITALITY, vitalityRow },
            { StatType.HUNGER, hungerRow },
            { StatType.TRUST, trustRow },
            // Ending stats
            { StatType.PYSICAL, physicalRow },
            { StatType.MUSIC, musicRow },
            { StatType.ART, artRow },
            { StatType.SOCIAL, socialRow },
            { StatType.ACADEMIC, academicRow },
            { StatType.DOMESTIC, domesticRow }
        };

        toggleButton.GetComponent<Button>().onClick.AddListener(OnclickToggleButton);
    }

    public void Initialize(List<Stat> stats)
    {
        foreach (var stat in stats)
        {
            if (rowDict.TryGetValue(stat.statType, out var row))
            {
                if(stat.statType == StatType.PYSICAL || stat.statType == StatType.MUSIC || stat.statType == StatType.ART ||
                    stat.statType == StatType.SOCIAL || stat.statType == StatType.ACADEMIC || stat.statType == StatType.DOMESTIC)
                {
                    row.Bind(stat, true);
                }
                else
                {
                    row.Bind(stat, false);
                }
            }
        }
    }

    public void OnclickToggleButton()
    {
        if(isExpanded)
        {
            Collapse();
        }
        else
        {
            Expand();
        }
    }

    private void Expand()
    {
        toggleButton.SetActive(false);
        StartCoroutine(ExpandCoroutine());
        StartCoroutine(ShowEndingStats());
    }

    private void Collapse()
    {
        toggleButton.SetActive(false);
        StartCoroutine(CollapseCoroutine());
        StartCoroutine(HideEndingStats());
    }

    private IEnumerator ExpandCoroutine()
    {
        float currentHeight = this.GetComponent<RectTransform>().sizeDelta.y;
        float targetHeight = 640.0f;
        while (currentHeight < targetHeight)
        {
            currentHeight += Time.deltaTime * 750;
            SetWindowHeight(currentHeight);
            yield return null;
        }
        SetWindowHeight(targetHeight);
        isExpanded = true;
        SwitchToggleButton();
    }

    private IEnumerator CollapseCoroutine()
    {
        float currentHeight = this.GetComponent<RectTransform>().sizeDelta.y;
        float targetHeight = 350.0f;
        while (currentHeight > targetHeight)
        {
            currentHeight -= Time.deltaTime * 750;
            SetWindowHeight(currentHeight);
            yield return null;
        }
        SetWindowHeight(targetHeight);
        isExpanded = false;
        SwitchToggleButton();
    }

    private void SetWindowHeight(float y)
    {
        RectTransform rectTransform = this.GetComponent<RectTransform>();
        Vector2 currentSize = rectTransform.sizeDelta;
        currentSize.y = y;
        rectTransform.sizeDelta = currentSize;
    }

    private void SwitchToggleButton()
    {
        if(isExpanded)
        {
            toggleButton.transform.rotation = Quaternion.Euler(0, 0, -90);
            toggleButton.transform.localPosition = new Vector3(0, -610, 0);
        }
        else
        {
            toggleButton.transform.rotation = Quaternion.Euler(0, 0, 90);
            toggleButton.transform.localPosition = new Vector3(0, -320, 0);
        }
        toggleButton.SetActive(true);
    }

    private IEnumerator ShowEndingStats()
    {
        CanvasGroup group = endingStatTexts.GetComponent<CanvasGroup>();        
        endingStatTexts.gameObject.SetActive(true);
        group.alpha = 0f;
        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime * 2f;
            group.alpha = t;
            yield return null;
        }
        group.alpha = 1;
    }

    private IEnumerator HideEndingStats()
    {
        RectTransform rect = endingStatTexts.GetComponent<RectTransform>();
        CanvasGroup group = endingStatTexts.GetComponent<CanvasGroup>();
        group.alpha = 1;
        float t = 1f;
        while (t > 0.0f)
        {
            t -= Time.deltaTime * 2f;
            group.alpha = t;
            yield return null;
        }
        group.alpha = 0;
        endingStatTexts.gameObject.SetActive(false);
    }
}
