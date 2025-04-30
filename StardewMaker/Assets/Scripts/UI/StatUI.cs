using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatUI : MonoBehaviour
{
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

    public GameObject toggleButton;
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
                Debug.Log($"Binding {stat.statType} to {row.name}");
                row.Bind(stat);
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
        RectTransform rectTransform = this.GetComponent<RectTransform>();
        Vector2 currentSize = rectTransform.sizeDelta;
        currentSize.y = 640;
        rectTransform.sizeDelta = currentSize;
    }

    private void Expand()
    {
        toggleButton.SetActive(false);
        StartCoroutine(ExpandCoroutine());
    }

    private void Collapse()
    {
        toggleButton.SetActive(false);
        StartCoroutine(CollapseCoroutine());
    }

    private IEnumerator ExpandCoroutine()
    {
        float currentHeight = this.GetComponent<RectTransform>().sizeDelta.y;
        float targetHeight = 640.0f;
        while (currentHeight < targetHeight)
        {
            currentHeight += Time.deltaTime;
            SetWindowHeight(currentHeight);
            yield return null;
        }
        isExpanded = true;
    }

    private IEnumerator CollapseCoroutine()
    {
        float currentHeight = this.GetComponent<RectTransform>().sizeDelta.y;
        float targetHeight = 350.0f;
        while (currentHeight < targetHeight)
        {
            currentHeight -= Time.deltaTime;
            SetWindowHeight(currentHeight);
            yield return null;
        }
        isExpanded = false;
    }

    private void SetWindowHeight(float y)
    {
        RectTransform rectTransform = this.GetComponent<RectTransform>();
        Vector2 currentSize = rectTransform.sizeDelta;
        currentSize.y = y;
        rectTransform.sizeDelta = currentSize;
    }
    // 키는중 끄는중
    // 키면 글자 
    // 글자 없애고 작아지기
}
