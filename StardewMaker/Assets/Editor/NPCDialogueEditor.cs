using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;
using NPC;

public class NPCDialogueEditor : EditorWindow
{
    private Dialogue currentNPC;           // 현재 편집 중인 NPC
    private int currentSequenceIndex = 0;     // 현재 보고 있는 시퀀스 인덱스

    private static string dialogueFolder = "Assets/Resources/Dialogues/NPCDialogues/";

    private string currentFileName;
    private string previousFileName;

    [MenuItem("Tools/NPC Dialogue Editor")]
    public static void ShowWindow()
    {
        GetWindow<NPCDialogueEditor>("NPC Dialogue Editor");
    }

    private Vector2 scrollPos;

    private void OnGUI()
    {
        GUILayout.Space(10);
        DrawNPCControls();

        if (currentNPC == null)
            return;

        GUILayout.Space(10);

        scrollPos = EditorGUILayout.BeginScrollView(scrollPos);

        DrawSequenceNavigator();

        if (currentNPC.dialogues.Count > 0)
        {
            DrawCurrentSequence();
        }

        EditorGUILayout.EndScrollView();

        GUILayout.Space(10);
        DrawSaveLoadButtons();
    }

    #region GUI Sections

    private void DrawNPCControls()
    {
        if (currentNPC == null)
        {
            EditorGUILayout.HelpBox("JSON을 불러오거나 새 다이얼로그를 생성하세요.", MessageType.Info);

            if (GUILayout.Button("새 다이얼로그 만들기"))
            {
                currentNPC = new Dialogue
                {
                    name = "새 NPC(한글명)",
                    dialogues = new List<DialogueSequence>()
                };
                previousFileName = null;
                currentFileName = "NewDialogue";
                currentSequenceIndex = 0;
            }

            if (GUILayout.Button("JSON 불러오기"))
            {
                string path = EditorUtility.OpenFilePanel("Load NPC Dialogue JSON", dialogueFolder, "json");
                if (!string.IsNullOrEmpty(path))
                {
                    string json = File.ReadAllText(path);
                    currentNPC = JsonUtility.FromJson<Dialogue>(json);

                    previousFileName = Path.GetFileNameWithoutExtension(path);
                    currentFileName = previousFileName;

                    currentSequenceIndex = 0;
                }
            }

            return;
        }
        currentFileName = EditorGUILayout.TextField("JSON 파일명(ID)", currentFileName);
        currentNPC.name = EditorGUILayout.TextField("NPC Name", currentNPC.name);
    }

    private void DrawSequenceNavigator()
    {
        if (currentNPC.dialogues.Count == 0)
        {
            EditorGUILayout.HelpBox("시퀸스가 없습니다.", MessageType.Warning);
            if (GUILayout.Button("시퀸스 추가"))
            {
                currentNPC.dialogues.Add(new DialogueSequence
                {
                    sequenceType = DialogueSequenceType.Greeting, // 기본값
                    customSequenceType = string.Empty,
                    lines = new List<DialogueLine>()
                });
                currentSequenceIndex = currentNPC.dialogues.Count - 1;
            }
            return;
        }

        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("<", GUILayout.Width(30)))
        {
            currentSequenceIndex = Mathf.Max(0, currentSequenceIndex - 1);
            GUI.FocusControl(null); // TextField 포커스 해제
            Repaint();
        }

        EditorGUILayout.LabelField($"{currentSequenceIndex + 1} / {currentNPC.dialogues.Count}", GUILayout.Width(60));

        if (GUILayout.Button(">", GUILayout.Width(30)))
        {
            currentSequenceIndex = Mathf.Min(currentNPC.dialogues.Count - 1, currentSequenceIndex + 1);
            GUI.FocusControl(null); // TextField 포커스 해제
            Repaint();
        }

        if (GUILayout.Button("추가", GUILayout.Width(50)))
        {
            currentNPC.dialogues.Add(new DialogueSequence
            {
                sequenceType = DialogueSequenceType.Greeting, // 기본값
                customSequenceType = string.Empty,
                lines = new List<DialogueLine>()
            });
            currentSequenceIndex = currentNPC.dialogues.Count - 1;
        }

        if (GUILayout.Button("삭제", GUILayout.Width(50)))
        {
            if (currentNPC.dialogues.Count > 0)
            {
                currentNPC.dialogues.RemoveAt(currentSequenceIndex);
                currentSequenceIndex = Mathf.Clamp(currentSequenceIndex, 0, currentNPC.dialogues.Count - 1);
            }
        }

        EditorGUILayout.EndHorizontal();
    }
    private void DrawCurrentSequence()
    {
        DialogueSequence seq = currentNPC.dialogues[currentSequenceIndex];

        // 시퀀스 키 선택
        DrawSequence(seq);

        if (seq.lines.Count == 0)
        {
            if (GUILayout.Button("대사 추가"))
            {
                seq.lines.Add(new DialogueLine { isSelf = true, speaker = currentNPC.name, text = "", actions = new DialogueAction() });
                seq.currentLineIndex = 0;
            }
            return;
        }

        // 대사 네비게이션
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("<", GUILayout.Width(30)))
        {
            seq.currentLineIndex = Mathf.Max(0, seq.currentLineIndex - 1);
            GUI.FocusControl(null); // 입력 포커스 해제 (TextArea 입력 중 변경 시 반영 안 되는 문제 방지)
            Repaint();              // 강제 UI 갱신
        }

        EditorGUILayout.LabelField($"{seq.currentLineIndex + 1}/{seq.lines.Count}", GUILayout.Width(60));

        if (GUILayout.Button(">", GUILayout.Width(30)))
        {
            seq.currentLineIndex = Mathf.Min(seq.lines.Count - 1, seq.currentLineIndex + 1);
            GUI.FocusControl(null);
            Repaint();
        }
        EditorGUILayout.EndHorizontal();

        // 현재 대사 표시
        DialogueLine line = seq.lines[seq.currentLineIndex];
        EditorGUILayout.BeginVertical("box");

        // 발화자 체크박스
        line.isSelf = EditorGUILayout.Toggle("본인", line.isSelf);

        // 본인이면 speaker를 고정
        if (line.isSelf)
        {
            line.speaker = currentNPC.name;
        }

        // TextField 활성/비활성 제어
        GUI.enabled = !line.isSelf; // 본인이면 비활성화
        line.speaker = EditorGUILayout.TextField("발화자", line.speaker);
        GUI.enabled = true; // 원래 상태로 복구

        EditorGUILayout.LabelField("대사");
        line.text = EditorGUILayout.TextArea(line.text, GUILayout.Height(80));

        // 표정 체크박스 + 드롭다운
        line.actions.useExpression = EditorGUILayout.Toggle("표정 변화", line.actions.useExpression);
        if (line.actions.useExpression)
        {
            line.actions.expression =
                (NpcEmotion)EditorGUILayout.EnumPopup("표정", line.actions.expression);
        }

        // 효과음 체크박스 + 드롭다운
        line.actions.useSFX = EditorGUILayout.Toggle("효과음", line.actions.useSFX);
        if (line.actions.useSFX)
        {
            string[] sfxOptions = new string[] { "Click", "Ding", "Explosion", "None" };
            int selectedSFX = Mathf.Max(0, System.Array.IndexOf(sfxOptions, line.actions.sfx));
            selectedSFX = EditorGUILayout.Popup("효과음", selectedSFX, sfxOptions);
            line.actions.sfx = sfxOptions[selectedSFX];
        }

        EditorGUILayout.EndVertical();

        // 삭제/추가 버튼
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("삭제"))
        {
            seq.lines.RemoveAt(seq.currentLineIndex);
            seq.currentLineIndex = Mathf.Clamp(seq.currentLineIndex, 0, seq.lines.Count - 1);
            Repaint();
        }
        if (GUILayout.Button("대사 추가"))
        {
            seq.lines.Insert(seq.currentLineIndex + 1, new DialogueLine { isSelf = true, speaker = currentNPC.name, text = "", actions = new DialogueAction() });
            seq.currentLineIndex++;
            Repaint();
        }
        EditorGUILayout.EndHorizontal();
    }

    private bool showRequireFoldout = false;
    private bool showForbiddenFoldout = false;

    private void DrawSequence(DialogueSequence seq)
    {
        EditorGUILayout.LabelField("시퀀스 키");

        // EnumPopup으로 enum 값 선택
        seq.sequenceType = (DialogueSequenceType)EditorGUILayout.EnumPopup(seq.sequenceType);

        // Custom일 때만 직접 입력 필드 표시
        if (seq.sequenceType == DialogueSequenceType.Custom)
        {
            if (string.IsNullOrEmpty(seq.customSequenceType))
                seq.customSequenceType = "";

            seq.customSequenceType = EditorGUILayout.TextField("직접 입력", seq.customSequenceType);
        }

        // require 태그 수정 박스
        GUILayout.Space(8);
        showRequireFoldout = EditorGUILayout.Foldout(showRequireFoldout, "Require Tags");

        if (showRequireFoldout)
        {
            EditorGUI.indentLevel++;

            EditorGUILayout.BeginVertical("box");

            string requireStr = string.Join(", ", seq.requireTags ?? new List<string>());
            string newRequire = EditorGUILayout.TextArea(requireStr, GUILayout.Height(50));

            if (newRequire != requireStr)
                seq.requireTags = ParseTagString(newRequire);

            EditorGUILayout.EndVertical();

            EditorGUI.indentLevel--;
        }

        GUILayout.Space(5);

        // forbidden 태그 수정 박스
        showForbiddenFoldout = EditorGUILayout.Foldout(showForbiddenFoldout, "Forbidden Tags");

        if (showForbiddenFoldout)
        {
            EditorGUI.indentLevel++;

            EditorGUILayout.BeginVertical("box");

            string forbidStr = string.Join(", ", seq.forbiddenTags ?? new List<string>());
            string newForbid = EditorGUILayout.TextArea(forbidStr, GUILayout.Height(50));

            if (newForbid != forbidStr)
                seq.forbiddenTags = ParseTagString(newForbid);

            EditorGUILayout.EndVertical();

            EditorGUI.indentLevel--;
        }
    }
    private List<string> ParseTagString(string input)
    {
        var result = new List<string>();

        if (string.IsNullOrWhiteSpace(input))
            return result;

        string[] split = input.Split(',');

        foreach (var s in split)
        {
            string t = s.Trim();

            if (!string.IsNullOrEmpty(t))
                result.Add(t.ToLower());
        }

        return result;
    }

    private void DrawSaveLoadButtons()
    {
        EditorGUILayout.Space();
        EditorGUILayout.BeginHorizontal();

        if (GUILayout.Button("저장"))
        {
            SaveJson(currentNPC);
        }

        if (GUILayout.Button("불러오기"))
        {
            string path = EditorUtility.OpenFilePanel("Load NPC Dialogue JSON", dialogueFolder, "json");
            if (!string.IsNullOrEmpty(path))
            {
                previousFileName = Path.GetFileNameWithoutExtension(path);

                string json = File.ReadAllText(path);
                currentNPC = JsonUtility.FromJson<Dialogue>(json);
                currentSequenceIndex = 0;
            }
        }

        EditorGUILayout.EndHorizontal();
    }

    #endregion

    #region Save / Load

    private void SaveJson(Dialogue dialogue)
    {
        if (!Directory.Exists(dialogueFolder))
        {
            Directory.CreateDirectory(dialogueFolder);
        }

        string fileName = currentFileName;
        string newPath = Path.Combine(dialogueFolder, $"{fileName}.json");

        string json = JsonUtility.ToJson(dialogue, true);

        if (!string.IsNullOrEmpty(previousFileName))
        {
            string oldPath = Path.Combine(dialogueFolder, $"{previousFileName}.json");
            if (File.Exists(oldPath) && oldPath != newPath)
            {
                File.Delete(oldPath);
                Debug.Log($"Renamed dialogue file from {oldPath} to {newPath}");
            }
        }

        File.WriteAllText(newPath, json);
        AssetDatabase.Refresh();
        Debug.Log($"Saved Dialogue: {newPath}");

        previousFileName = fileName;
    }

    #endregion
}




