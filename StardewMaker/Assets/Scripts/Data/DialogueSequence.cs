using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DialogueSequence
{
    public string key;               // "intro", "quest_start" 등
    public List<DialogueLine> lines; // 대사 리스트
    public int currentLineIndex = 0; // 현재 편집 중인 대사
}
