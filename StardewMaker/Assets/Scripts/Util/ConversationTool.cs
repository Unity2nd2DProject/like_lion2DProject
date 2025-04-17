using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

public enum DialogueType
{
    NORMAL,
    CHOICE,
    OPTION,
    NONE
}

public enum EmotionType
{
    IDLE,
    SAD,
    HAPPY,
    ANGRY,
    SLEEPY,
    SEEK,
    NONE
}

public enum DialougeDataType
{
    TYPE,
    NAME,
    EMOTION,
    NEXT_ID,
    OPTION_ID_LIST,
    EXT1,
    KOREAN,
    ENGLISH
}

public class Dialogue
{
    public int id;
    public DialogueType type;
    public string name;
    public EmotionType emotion;
    public int nextId;
    public List<string> optionIdList;
    public string ext1;
    public string korean;
    public string english;
}


public static class ConversationTool
{
    private static string TAG = "[ConversationTool]";
    private static string type = "Type";
    private static string name = "Name";
    private static string emotion = "Emotion";
    private static string nextId = "NextId";
    private static string optionIdList = "OptionIdList";
    private static string ext1 = "Ext1";
    private static string korean = "Korean";
    private static string english = "English";
    public static List<string> dialogDataList = new() { type, name, emotion, nextId, optionIdList, ext1, korean, english };

    // public static Dictionary<string, Dictionary<string, string>> dic = new();
    public static Dictionary<int, Dialogue> dic = new();

    public static void CsvRead(string fileName)
    {
        TextAsset csvFile = Resources.Load<TextAsset>(fileName); using (StringReader sr = new StringReader(csvFile.text))
        {
            Dictionary<int, int> dialogDataIndexDic = new();
            string[] langs = sr.ReadLine().Split(","); // 언어들 받아오기 Key,Id,Emotion,Korean(ko),English(en)
            // Debug.Log($"{TAG} langs.Length {langs.Length}");
            for (int i = 0; i < langs.Length; i++) // 딕셔너리에 저장.
            {
                // Debug.Log($"{TAG} langs.Length {langs[i]}");
                if (langs[i].Contains(type)) dialogDataIndexDic.Add((int)DialougeDataType.TYPE, i);
                else if (langs[i].Contains(name)) dialogDataIndexDic.Add((int)DialougeDataType.NAME, i);
                else if (langs[i].Contains(emotion)) dialogDataIndexDic.Add((int)DialougeDataType.EMOTION, i);
                else if (langs[i].Contains(nextId)) dialogDataIndexDic.Add((int)DialougeDataType.NEXT_ID, i);
                else if (langs[i].Contains(optionIdList)) dialogDataIndexDic.Add((int)DialougeDataType.OPTION_ID_LIST, i);
                else if (langs[i].Contains(ext1)) dialogDataIndexDic.Add((int)DialougeDataType.EXT1, i);
                else if (langs[i].Contains(SystemLanguage.Korean.ToString())) dialogDataIndexDic.Add((int)DialougeDataType.KOREAN, i);
                else if (langs[i].Contains(SystemLanguage.English.ToString())) dialogDataIndexDic.Add((int)DialougeDataType.ENGLISH, i);
            }

            int lineNum = 2;
            string line;
            while ((line = sr.ReadLine()) != null)
            {
                string[] parts = ParseCsvLine(line);

                string ty = parts[dialogDataIndexDic[(int)DialougeDataType.TYPE]];
                string em = parts[dialogDataIndexDic[(int)DialougeDataType.EMOTION]];
                string nId = parts[dialogDataIndexDic[(int)DialougeDataType.NEXT_ID]];

                // Debug.Log($"{TAG} lineNum {lineNum}");
                Dialogue dialogue = new()
                {
                    id = lineNum,
                    type = Enum.TryParse(ty, out DialogueType a) ? a : DialogueType.NONE,
                    name = parts[dialogDataIndexDic[(int)DialougeDataType.NAME]],
                    emotion = Enum.TryParse(em, out EmotionType b) ? b : EmotionType.NONE,
                    nextId = int.TryParse(nId, out int c) ? c : -1,
                    optionIdList = parts[dialogDataIndexDic[(int)DialougeDataType.OPTION_ID_LIST]].Split(',').ToList(),
                    korean = parts[dialogDataIndexDic[(int)DialougeDataType.KOREAN]],
                    english = parts[dialogDataIndexDic[(int)DialougeDataType.ENGLISH]],
                };

                dic[lineNum++] = dialogue;
            }
        }

        // 데이터 체크용
        // foreach (var entry in dic)
        // {
        //     var compositeKey = entry.Key;
        //     var dialog = entry.Value;

        //     Debug.Log($"Key: {compositeKey}");
        //     Debug.Log($"id: {dialog.id}, type: {dialog.type}, Name: {dialog.name}, Korean: {dialog.korean}");
        // }
    }

    public static string[] ParseCsvLine(string line) // CSV 파싱
    {
        var csvPattern = @"(?:^|,)(?:(?:""(?<value>(?:[^""]|"""")*)"")|(?<value>[^,]*))(?=,|$)";

        var matches = Regex.Matches(line, csvPattern);
        var values = new List<string>();

        foreach (Match match in matches)
        {
            string value = match.Groups["value"].Value;
            values.Add(value.Replace("\"\"", "\""));
        }

        int expectedFieldCount = line.Count(c => c == ',') + 1;
        while (values.Count < expectedFieldCount) // 빈 필드 보정
        {
            values.Add("");
        }

        return values.ToArray();
    }

    public static string GetName(int key)
    {
        return dic[key].name;
    }

    public static EmotionType GetEmotion(int key)
    {
        return dic[key].emotion;
    }

    public static string GetKorean(int key)
    {
        return dic[key].korean;
    }

    public static string GetEnglish(int key)
    {
        return dic[key].english;
    }
}
