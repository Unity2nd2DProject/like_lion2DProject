using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using UnityEditor.PackageManager.Requests;
using Unity.VisualScripting;

public class DialogueController : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    [SerializeField] private GameObject buttonGrid;
    [SerializeField] private GameObject questPopup;
    [SerializeField] private GameObject npcImageObject;
    private Image npcImage;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private Button okButton;


    private void Start()
    {
        Hide();
    }

    public void Show(Sprite npcIllustration, string npcName, string text, QuestDataSO questData = null, Action onOK = null, Action onCancel = null)
    {
        panel.SetActive(true);
        buttonGrid.SetActive(true);
        npcImageObject.SetActive(true);
        npcImage.sprite = npcIllustration;

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

        // 버튼 표시 여부 결정
        bool hasOkAction = onOK != null;
        okButton.gameObject.SetActive(hasOkAction);
    }

    public void Hide()
    {
        buttonGrid.SetActive(false);
        npcImageObject.SetActive(false);
        panel.SetActive(false);
        questPopup.SetActive(false);
    }
}
