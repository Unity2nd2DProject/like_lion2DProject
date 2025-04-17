using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

public enum DialogDataType
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

public enum DialogType
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

public class Dialog
{
    public int id;
    public DialogType type;
    public string name;
    public EmotionType emotion;
    public int nextId;
    public List<int> optionIdList;
    public string ext1;
    public string korean;
    public string english;

    public string GetText()
    {
        // todo 언어 설정한 뒤 그 언어로 반환
        return korean;
    }
}

public static class DialogTool
{
    private static string TAG = "[DialogTool]";
    private static string type = "Type";
    private static string name = "Name";
    private static string emotion = "Emotion";
    private static string nextId = "NextId";
    private static string optionIdList = "OptionIdList";
    private static string ext1 = "Ext1";
    private static string korean = "Korean";
    private static string english = "English";
    public static List<string> dialogDataList = new() { type, name, emotion, nextId, optionIdList, ext1, korean, english };

    public static Dictionary<int, Dialog> dic = new();

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
                if (langs[i].Contains(type)) dialogDataIndexDic.Add((int)DialogDataType.TYPE, i);
                else if (langs[i].Contains(name)) dialogDataIndexDic.Add((int)DialogDataType.NAME, i);
                else if (langs[i].Contains(emotion)) dialogDataIndexDic.Add((int)DialogDataType.EMOTION, i);
                else if (langs[i].Contains(nextId)) dialogDataIndexDic.Add((int)DialogDataType.NEXT_ID, i);
                else if (langs[i].Contains(optionIdList)) dialogDataIndexDic.Add((int)DialogDataType.OPTION_ID_LIST, i);
                else if (langs[i].Contains(ext1)) dialogDataIndexDic.Add((int)DialogDataType.EXT1, i);
                else if (langs[i].Contains(SystemLanguage.Korean.ToString())) dialogDataIndexDic.Add((int)DialogDataType.KOREAN, i);
                else if (langs[i].Contains(SystemLanguage.English.ToString())) dialogDataIndexDic.Add((int)DialogDataType.ENGLISH, i);
            }

            int lineNum = 2;
            string line;
            while ((line = sr.ReadLine()) != null)
            {
                string[] rows = ParseCsvLine(line); // 한 줄을 읽어서 string 배열에 넣음

                string ty = rows[dialogDataIndexDic[(int)DialogDataType.TYPE]];
                string em = rows[dialogDataIndexDic[(int)DialogDataType.EMOTION]];
                string nId = rows[dialogDataIndexDic[(int)DialogDataType.NEXT_ID]];
                string opList = rows[dialogDataIndexDic[(int)DialogDataType.OPTION_ID_LIST]];

                Dialog dialog = new()
                {
                    id = lineNum,
                    type = Enum.TryParse(ty, out DialogType a) ? a : DialogType.NONE,
                    name = rows[dialogDataIndexDic[(int)DialogDataType.NAME]],
                    emotion = Enum.TryParse(em, out EmotionType b) ? b : EmotionType.NONE,
                    nextId = int.TryParse(nId, out int c) ? lineNum + c : -1,
                    optionIdList = opList.Split(',').Select(s => int.TryParse(s.Trim(), out int result) ? lineNum + result : -1).ToList(),
                    korean = rows[dialogDataIndexDic[(int)DialogDataType.KOREAN]],
                    english = rows[dialogDataIndexDic[(int)DialogDataType.ENGLISH]],
                };

                // foreach (var i in dialogue.optionIdList)
                // {
                //     Debug.Log($"{TAG} dialogue.optionIdList {i}");
                // }

                dic[lineNum++] = dialog;
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

}
