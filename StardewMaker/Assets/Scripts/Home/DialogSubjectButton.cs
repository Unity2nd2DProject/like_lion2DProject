using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogSubjectButton : MonoBehaviour
{
    public static Action<SituationType> OnDialogSubjectButtonRequested;

    private Button button;
    [SerializeField] private TMP_Text text;
    private SituationType situationType;

    void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        OnDialogSubjectButtonRequested?.Invoke(situationType);
    }

    public void SetText(string text)
    {
        this.text.text = text;
    }

    public void SetSituationType(SituationType situationType)
    {
        this.situationType = situationType;
    }
}
