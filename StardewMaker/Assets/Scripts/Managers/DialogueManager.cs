using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using NPC;

[System.Serializable]
public class NpcSpritesSet
{
    public NPC.NpcId npcId;
    public Sprite neutral;
    public Sprite happy;
    public Sprite sad;
    public Sprite surprised;
}

public class DialogueManager : Singleton<DialogueManager>
{
    [SerializeField] private GameObject dialogUI;

    [SerializeField] private TypewriterEffect typewriterEffect;
    [SerializeField] private TextMeshProUGUI dialogueText;

    [SerializeField] private List<NpcSpritesSet> npcExpressions;

    protected override void Awake()
    {
        base.Awake();
    }

    public void StartDialogue(NpcId npcId)
    {
        SetDialogue(npcId);
        ShowDialogue();
    }

    public void ShowDialogue()
    {
        dialogUI.SetActive(true);
    }

    private void SetDialogue(NpcId npcId)
    {
        var npcSet = npcExpressions.Find(x => x.npcId == npcId);
        dialogUI.GetComponent<DialogueController>().SetDialogue(LoadDialogue(npcId), npcSet);
    }

    NPCDialogue LoadDialogue(NpcId npcId)
    {
        TextAsset jsonFile = Resources.Load<TextAsset>($"Datas/Dialogues/{npcId.ToString() + "_dialogue"}");
        if (jsonFile == null)
        {
            Debug.LogError($"Dialogue JSON not found");
            return null;
        }

        NPCDialogue dialogue = JsonUtility.FromJson<NPCDialogue>(jsonFile.text);
        return dialogue;
    }
}

