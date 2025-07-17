using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using UnityEditor.PackageManager.Requests;
using Unity.VisualScripting;

public class NPCDialgoueUI : Singleton<NPCDialgoueUI>
{
    [SerializeField] private GameObject panel;
    [SerializeField] private GameObject buttonGrid;
    [SerializeField] private GameObject questPopup;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private Button okButton;
    [SerializeField] private Button cancelButton;

    private Action onOK;
    private Action onCancel;

    private void Start()
    {
        okButton.onClick.AddListener(() => {
            onOK?.Invoke();
            Hide();
        });
        cancelButton.onClick.AddListener(() => {
            onCancel?.Invoke();
            Hide();
        });
        Hide();
    }

    public void Show(string npcName, string text, QuestData questData = null, Action onOK = null, Action onCancel = null)
    {
        panel.SetActive(true);
        buttonGrid.SetActive(true);
        if (questData != null)
        {
            questPopup.SetActive(true);
            QuestDetailPopupUI detailUI = questPopup.GetComponentInChildren<QuestDetailPopupUI>();
            detailUI.Show(questData);
        }

        nameText.text = npcName;
        TypewriterEffect typewriter = dialogueText.GetComponent<TypewriterEffect>();
        if (typewriter != null)
        {
            typewriter.fullText = text;
            typewriter.StartTyping();
        }
        else
        {
            dialogueText.text = text;
        }

        this.onOK = onOK;
        this.onCancel = onCancel;

        // 버튼 표시 여부 결정
        bool hasOkAction = onOK != null;
        okButton.gameObject.SetActive(hasOkAction);
        cancelButton.gameObject.SetActive(true); // 항상 표시
    }

    public void Hide()
    {
        buttonGrid.SetActive(false);
        panel.SetActive(false);
        questPopup.SetActive(false);
    }
}
