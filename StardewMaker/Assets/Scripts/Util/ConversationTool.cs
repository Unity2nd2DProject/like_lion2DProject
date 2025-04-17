using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

public enum LanguageType
{
    NAME,
    EMOTION,
    KOREAN,
    ENGLISH,
}

public enum EmotionType
{
    IDLE,
    SAD,
    ANGRY,
    SLEEPY,
    SEEK
}


public static class ConversationTool
{
    private static string TAG = "[ConversationTool]";
    private static string nameLang = "Name";
    private static string emotionLang = "Emotion";
    public static List<string> languageList = new() { nameLang, emotionLang, "Korean", "English" };
    public static Dictionary<string, Dictionary<string, string>> dic = new();

    public static void CsvRead(string fileName)
    {
        TextAsset csvFile = Resources.Load<TextAsset>(fileName);
        using (StringReader sr = new StringReader(csvFile.text))
        {
            Dictionary<int, int> langNameIndexDic = new();
            string[] langs = sr.ReadLine().Split(","); // 언어들 받아오기 Key,Id,Emotion,Korean(ko),English(en)
            // Debug.Log($"{TAG} langs.Length {langs.Length}");
            for (int i = 0; i < langs.Length; i++) // 딕셔너리에 저장.
            {
                // Debug.Log($"{TAG} langs.Length {langs[i]}");
                if (langs[i].Contains(nameLang)) langNameIndexDic.Add((int)LanguageType.NAME, i); // 0:2
                else if (langs[i].Contains(emotionLang)) langNameIndexDic.Add((int)LanguageType.EMOTION, i); // 1:3
                else if (langs[i].Contains(SystemLanguage.Korean.ToString())) langNameIndexDic.Add((int)LanguageType.KOREAN, i); // 2:4
                else if (langs[i].Contains(SystemLanguage.English.ToString())) langNameIndexDic.Add((int)LanguageType.ENGLISH, i); // 3:5
            }

            string line;
            while ((line = sr.ReadLine()) != null)
            {
                string[] parts = ParseCsvLine(line); // ["안녕, 아빠!",,딸,0,"안녕, 아빠!","Hi, Dad!"]

                Dictionary<string, string> d = new();
                foreach (var kv in langNameIndexDic)
                {
                    string languageName = languageList[kv.Key]; // 0
                    string content = parts[kv.Value]; // 2
                    d[languageName] = content; // {Name:딸}
                }
                dic[parts[0]] = d; // 안녕, 아빠!:{Name:딸}, {Korean:안녕, 아빠!}
            }
        }

        // 데이터 체크용
        // foreach (var entry in dic)
        // {
        //     var compositeKey = entry.Key;
        //     var translations = entry.Value;

        //     Debug.Log($"Key: {compositeKey}");
        //     Debug.Log($"English: {translations["English"]}, Korean: {translations["Korean"]}, Name: {translations["Name"]}");
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

    public static string GetName(string key)
    {
        return dic[key][nameLang];
    }

    public static int GetEmotion(string key)
    {
        return int.Parse(dic[key][emotionLang]);
    }

    public static string GetKorean(string key)
    {
        return dic[key][languageList[(int)LanguageType.KOREAN]];
    }

    public static string GetEnglish(string key)
    {
        return dic[key][languageList[(int)LanguageType.ENGLISH]];
    }
}
