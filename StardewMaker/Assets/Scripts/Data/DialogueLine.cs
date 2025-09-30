using UnityEngine;


[System.Serializable]
public class DialogueLine
{
    public bool isSelf = true;        // 체크박스 "본인"
    public string speakerId;          // 발화자 ID
    public string text;               // 대사 내용
    public DialogueAction actions;   // 표정, 효과음 등
}
