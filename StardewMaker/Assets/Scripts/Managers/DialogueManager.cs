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
    private DialogueController dialogueController;
    [SerializeField] private List<NpcSpritesSet> npcExpressions;

    Dialogue currentDialogue;

    protected override void Awake()
    {
        base.Awake();
        dialogueController = UIManager.Instance.dialogueUI.GetComponent<DialogueController>();
    }

    public void LoadStoryDialogue(StoryID storyID)
    {
        // currentDialogue = JsonUtility.FromJson(Resources.Load<TextAsset>($"Dialogues/Story/{storyID}"));
    }

    public void StartNPCDialogue(NPCController npc)
    {
        
    }

    public void StartStoryDialogue(StoryID storyID,int storyIndex)
    {

    }


}
