using UnityEngine;

[System.Serializable]
public class DialogueAction
{
    public bool useExpression = false;
    public NPC.NpcEmotion expression;   // 존재하는 표정 리스트에서 선택
    public bool useSFX = false;
    public string sfx;          // 존재하는 효과음 리스트에서 선택
}