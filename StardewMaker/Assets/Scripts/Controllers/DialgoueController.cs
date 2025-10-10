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

    public void Start()
    {
        Hide();
    }

    public void SetDialogue(NPC.NpcId npcId, Sprite npcImage)
    {
        nameText.text = npcId.DisplayName();
        this.npcImage.sprite = npcImage;

    }

    public void Hide()
    {
        buttonGrid.SetActive(false);
        npcImageObject.SetActive(false);
        panel.SetActive(false);
        questPopup.SetActive(false);
    }
}
