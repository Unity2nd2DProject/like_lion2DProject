using NPC;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DialogueSequence
{
    public DialogueSequenceType sequenceType;     
    public string customSequenceType; // Custom 대사 시퀀스의 고유 키
    public List<DialogueLine> lines; // 대사 리스트
    public int currentLineIndex = 0; // 현재 편집 중인 대사
}
