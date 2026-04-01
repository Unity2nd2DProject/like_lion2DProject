using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;
using NPC;


[System.Serializable]
public class StoryDialogue
{
    public string storyName;
    public StoryID storyID;

    public List<StorySequence> sequences = new List<StorySequence>();
}

[System.Serializable]
public class StorySequence
{
    public List<StoryLine> lines = new List<StoryLine>();

    // 현재 라인 에디팅 인덱스 (Editor에서만 사용)
    public int currentLineIndex = 0;
}

[System.Serializable]
public class StoryLine
{
    public NPC.NpcId speaker;   // 화자는 enum
    public string text;     // 대사
    public DialogueAction actions = new DialogueAction();
}

public class StoryDialogueEditor : EditorWindow
{
    private StoryDialogue story;
    private int currentSequenceIndex = 0;

    private string fileName = "NewStoryDialog";
    private string previousFileName;

    private static string folder = "Assets/Resources/Dialogues/StoryDialogue/";

    private Vector2 scroll;

    [MenuItem("Tools/Story Dialogue Editor")]
    public static void Open()
    {
        GetWindow<StoryDialogueEditor>("Story Dialogue Editor");
    }

    private void OnGUI()
    {
        GUILayout.Space(10);

        DrawStoryControls();

        if (story == null)
            return;

        GUILayout.Space(10);

        scroll = EditorGUILayout.BeginScrollView(scroll);

        DrawSequenceNavigator();

        if (story.sequences.Count > 0)
            DrawCurrentSequence();

        EditorGUILayout.EndScrollView();

        GUILayout.Space(10);
        DrawSaveLoad();
    }

    // ------------------------------------------------------------------
    // Story Info (Top)
    // ------------------------------------------------------------------
    private void DrawStoryControls()
    {
        if (story == null)
        {
            EditorGUILayout.HelpBox("스토리 JSON을 불러오거나 새로 생성하세요.", MessageType.Info);

            if (GUILayout.Button("새 스토리 생성"))
            {
                story = new StoryDialogue
                {
                    storyName = "새 스토리",
                    sequences = new List<StorySequence>()
                };
                currentSequenceIndex = 0;
            }

            if (GUILayout.Button("JSON 불러오기"))
            {
                string path = EditorUtility.OpenFilePanel("Load Story", folder, "json");
                if (!string.IsNullOrEmpty(path))
                {
                    string json = File.ReadAllText(path);
                    story = JsonUtility.FromJson<StoryDialogue>(json);

                    previousFileName = Path.GetFileNameWithoutExtension(path);
                    fileName = previousFileName;

                    currentSequenceIndex = 0;
                }
            }

            return;
        }

        fileName = EditorGUILayout.TextField("파일명", fileName);
        story.storyName = EditorGUILayout.TextField("스토리명", story.storyName);
        story.storyID = (StoryID)EditorGUILayout.EnumPopup("스토리 ID", story.storyID);
    }

    // ------------------------------------------------------------------
    // Sequence
    // ------------------------------------------------------------------
    private void DrawSequenceNavigator()
    {
        if (story.sequences.Count == 0)
        {
            EditorGUILayout.HelpBox("시퀀스가 없습니다.", MessageType.Warning);

            if (GUILayout.Button("시퀀스 추가"))
            {
                story.sequences.Add(new StorySequence());
                currentSequenceIndex = 0;
            }
            return;
        }

        EditorGUILayout.BeginHorizontal();

        if (GUILayout.Button("<", GUILayout.Width(30)))
        {
            currentSequenceIndex = Mathf.Max(0, currentSequenceIndex - 1);
            GUI.FocusControl(null);
            Repaint();
        }

        EditorGUILayout.LabelField($"{currentSequenceIndex + 1} / {story.sequences.Count}", GUILayout.Width(70));

        if (GUILayout.Button(">", GUILayout.Width(30)))
        {
            currentSequenceIndex = Mathf.Min(story.sequences.Count - 1, currentSequenceIndex + 1);
            GUI.FocusControl(null);
            Repaint();
        }

        if (GUILayout.Button("추가", GUILayout.Width(50)))
        {
            story.sequences.Add(new StorySequence());
            currentSequenceIndex = story.sequences.Count - 1;
        }

        if (GUILayout.Button("삭제", GUILayout.Width(50)))
        {
            story.sequences.RemoveAt(currentSequenceIndex);
            currentSequenceIndex = Mathf.Clamp(currentSequenceIndex, 0, story.sequences.Count - 1);
        }

        EditorGUILayout.EndHorizontal();
    }

    // ------------------------------------------------------------------
    // Current Sequence
    // ------------------------------------------------------------------
    private void DrawCurrentSequence()
    {
        StorySequence seq = story.sequences[currentSequenceIndex];

        if (seq.lines.Count == 0)
        {
            if (GUILayout.Button("대사 추가"))
            {
                seq.lines.Add(CreateNewLine());
                seq.currentLineIndex = 0;
            }
            return;
        }

        // 라인 네비게이션
        EditorGUILayout.BeginHorizontal();

        if (GUILayout.Button("<", GUILayout.Width(30)))
        {
            seq.currentLineIndex = Mathf.Max(0, seq.currentLineIndex - 1);
            GUI.FocusControl(null);
            Repaint();
        }

        EditorGUILayout.LabelField($"{seq.currentLineIndex + 1} / {seq.lines.Count}", GUILayout.Width(70));

        if (GUILayout.Button(">", GUILayout.Width(30)))
        {
            seq.currentLineIndex = Mathf.Min(seq.lines.Count - 1, seq.currentLineIndex + 1);
            GUI.FocusControl(null);
            Repaint();
        }

        if (GUILayout.Button("추가"))
        {
            seq.lines.Insert(seq.currentLineIndex + 1, CreateNewLine());
            seq.currentLineIndex++;
            Repaint();
        }

        if (GUILayout.Button("삭제"))
        {
            seq.lines.RemoveAt(seq.currentLineIndex);
            seq.currentLineIndex = Mathf.Clamp(seq.currentLineIndex, 0, seq.lines.Count - 1);
            Repaint();
        }

        EditorGUILayout.EndHorizontal();

        DrawLine(seq.lines[seq.currentLineIndex]);
    }

    // ------------------------------------------------------------------
    // Single Line
    // ------------------------------------------------------------------
    private void DrawLine(StoryLine line)
    {
        EditorGUILayout.BeginVertical("box");

        line.speaker = (NpcId)EditorGUILayout.EnumPopup("화자", line.speaker);

        EditorGUILayout.LabelField("대사");
        line.text = EditorGUILayout.TextArea(line.text, GUILayout.Height(80));

        // 표정
        line.actions.useExpression = EditorGUILayout.Toggle("표정 변화", line.actions.useExpression);
        if (line.actions.useExpression)
            line.actions.expression = (NpcEmotion)EditorGUILayout.EnumPopup("표정", line.actions.expression);

        // 효과음
        line.actions.useSFX = EditorGUILayout.Toggle("효과음", line.actions.useSFX);
        if (line.actions.useSFX)
        {
            string[] options = { "Click", "Ding", "Explosion", "None" };
            int idx = Mathf.Max(0, System.Array.IndexOf(options, line.actions.sfx));
            idx = EditorGUILayout.Popup("효과음", idx, options);
            line.actions.sfx = options[idx];
        }

        EditorGUILayout.EndVertical();
    }

    private StoryLine CreateNewLine()
    {
        return new StoryLine
        {
            speaker = NpcId.None,
            text = "",
            actions = new DialogueAction()
        };
    }

    // ------------------------------------------------------------------
    // Save / Load
    // ------------------------------------------------------------------
    private void DrawSaveLoad()
    {
        EditorGUILayout.BeginHorizontal();

        if (GUILayout.Button("저장"))
            Save(story);

        if (GUILayout.Button("불러오기"))
        {
            string path = EditorUtility.OpenFilePanel("Load Story", folder, "json");
            if (!string.IsNullOrEmpty(path))
            {
                previousFileName = Path.GetFileNameWithoutExtension(path);
                string json = File.ReadAllText(path);
                story = JsonUtility.FromJson<StoryDialogue>(json);
                currentSequenceIndex = 0;
            }
        }

        EditorGUILayout.EndHorizontal();
    }

    private void Save(StoryDialogue data)
    {
        if (!Directory.Exists(folder))
            Directory.CreateDirectory(folder);

        string newPath = Path.Combine(folder, $"{fileName}.json");

        string json = JsonUtility.ToJson(data, true);

        // 기존 파일 삭제 (이름 변경 시)
        if (!string.IsNullOrEmpty(previousFileName))
        {
            string oldPath = Path.Combine(folder, $"{previousFileName}.json");
            if (File.Exists(oldPath) && oldPath != newPath)
                File.Delete(oldPath);
        }

        File.WriteAllText(newPath, json);
        AssetDatabase.Refresh();

        previousFileName = fileName;
        Debug.Log($"Saved Story: {newPath}");
    }
}