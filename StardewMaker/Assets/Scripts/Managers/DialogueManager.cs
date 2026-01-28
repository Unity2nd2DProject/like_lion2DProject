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
    private GameObject dialogUI;
    [SerializeField] private List<NpcSpritesSet> npcExpressions;

    protected override void Awake()
    {
        base.Awake();
        dialogUI = UIManager.Instance.dialogueUI;
    }

    public void StartDialogue(NPCController npc)
    {
        SetDialogue(npc);
    }

    public void StartStoryDialogue(StoryID storyID)
    {
        SetStoryDialogue(storyID);
    }

    private void SetDialogue(NPCController npc)
    {
        var npcSet = npcExpressions.Find(x => x.npcId == npc.npcID);
        dialogUI.GetComponent<DialogueController>().SetDialogue(LoadDialogue(npc.npcID), npcSet, npc);
    }

    private void SetStoryDialogue(StoryID storyID)
    {

    }

    NPCDialogue LoadDialogue(NpcId npcId)
    {
        TextAsset jsonFile = Resources.Load<TextAsset>($"Dialogues/{npcId.ToString() + "_Dialogue"}");
        if (jsonFile == null)
        {
            Debug.LogError($"Dialogue JSON not found");
            return null;
        }

        NPCDialogue dialogue = JsonUtility.FromJson<NPCDialogue>(jsonFile.text);
        return dialogue;
    }
}

