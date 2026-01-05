using NPC;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DialogueSequence
{
    public DialogueSequenceType sequenceType;
    public List<string> requireTags; // 대사 재생에 필요한 태그 리스트
    public List<string> forbiddenTags; // 있으면 재생되지 않는 태그 리스트
    public string customSequenceType; // Custom 대사 시퀀스의 고유 키
    public List<DialogueLine> lines; // 대사 리스트
    public int currentLineIndex = 0; // 현재 편집 중인 대사
}
