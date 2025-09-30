using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class NPCDialogue
{
    public string dialogueId;
    public string name;
    public List<DialogueSequence> dialogues; // Dictionary -> List
}

