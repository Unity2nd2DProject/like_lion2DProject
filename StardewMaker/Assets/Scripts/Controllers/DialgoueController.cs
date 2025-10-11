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
    [SerializeField] private GameObject npcImageObject;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI dialogueText;

    private NPCDialogue currentDialogue;

    public void Start()
    {
        Hide();
    }

    public void SetDialogue(NPCDialogue npcDialogue, NpcSpritesSet npcSpriteSet)
    {
        currentDialogue = npcDialogue;
    }

    public void Hide()
    {
        buttonGrid.SetActive(false);
        npcImageObject.SetActive(false);
        panel.SetActive(false);
    }
}