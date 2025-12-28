using UnityEngine;


[System.Serializable]
public class DialogueLine
{
    public bool isSelf = true;        // 체크박스 "본인"
    public string speaker;          // 발화자 이름
    public string text;               // 대사 내용
    public DialogueAction actions;   // 표정, 효과음 등
}
