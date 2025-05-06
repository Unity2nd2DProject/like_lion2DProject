using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

public enum DialogDataType
{
    NONE,
    TYPE, // NORMAL
    NAME, // 딸
    SITUATION, // MORNING
    CONDITION_TYPE, // 호감도
    CONDITION_OPERATOR, // =
    CONDITION_VALUE, // 3
    EMOTION, // HAPPY
    NEXT_ID,
    OPTION_ID_LIST,
    EXT1,
    TARGET_TYPE,
    TARGET_OPERATOR,
    TARGET_VALUE,
    SCHEDULE_TYPE,
    KOREAN,
    ENGLISH
}

public enum DialogType
{
    NONE,
    NORMAL,
    CHOICE,
    OPTION,
    CONDITION,
    MULTI,
}

public enum MultiOptionIdType
{
    FIRST,
    SECOND
}

public enum NameType
{
    NONE,
    SYSTEM,
    PRINCESS,
    FATHER,
    MERCHANT
}

public enum SituationType
{
    NONE,
    INTRO,
    MORNING,
    EVENING,
    EVENT,
    BOOK,
    COOK,
    WANT_TO_EAT,
    WANT_TO_DO,
    WANT_TO_BE,
    MEMORY,
    SWEET,
    TODAY,
    SLEEP,
    GIFT,
    GIFT_RECEIVED
}

public enum ConditionType
{
    NONE,
    MOOD,
    VITALITY,
    HUNGER,
    TRUST,

    PYSICAL,
    MUSIC,
    ART,
    SOCIAL,
    ACADEMIC,
    DOMESTIC,

    TIME

}

public enum EmotionType
{
    NONE,
    IDLE,
    HAPPY,
    ANGRY,
    SAD,
    SLEEPY,
    SEEK
}

public enum ExtType
{
    NONE,
    ACT,
    EXIT,
    SHOP,
    WILL,
    SLEEP,
    COOK,
    GIFT
}

public enum ScheduleType
{
    NONE,
    EXERCISE,
    HOME_WORK,
    PLAY,
    READ_BOOK,
    DRAWING,
    COOK,
    MUSIC,
    DOLL
}

public class Dialog
{
    private string TAG = "[Dialog]";

    public static Action<string> OnStatChangeRequested;
    public int id;
    public DialogType type;
    public NameType nameType;
    public SituationType situationType;
    public ConditionType conditionType;
    public string conditionOperator;
    public int conditionValue;
    public EmotionType emotion;
    public int nextId;
    public List<int> optionIdList;
    public ExtType ext1;
    public ConditionType targetType;
    public string targetOperator;
    public int targetValue;
    public ScheduleType scheduleType;
    public List<string> korean;
    public string english;

    public Dictionary<ConditionType, string> conditionStrDic = new(){
        {ConditionType.MOOD, "기분"},
        {ConditionType.VITALITY, "건강"},
        {ConditionType.HUNGER, "배고픔"},
        {ConditionType.TRUST, "신뢰도"},
    };

    public bool IsConditionMetNotUse(int value)
    {
        switch (conditionOperator)
        {
            case "<":
                return value < conditionValue;
            case "<=":
                return value <= conditionValue;
            case "==":
                return value == conditionValue;
            case ">=":
                return value >= conditionValue;
            case ">":
                return value > conditionValue;
            case "!=":
                return value != conditionValue;
            default:
                return false;
        }
    }

    public bool IsConditionMet(List<Stat> stats)
    {
        if (conditionType == ConditionType.NONE) return true;
        foreach (var i in stats)
        {
            if (ConditionType.TIME == conditionType)
            {
                // Debug.Log($"{TAG} currentHour {TimeManager.Instance.currentHour}");
                // Debug.Log($"{TAG} conditionValue {conditionValue}");
                switch (conditionOperator)
                {
                    case "<":
                        return TimeManager.Instance.currentHour < conditionValue;
                    case "<=":
                        return TimeManager.Instance.currentHour <= conditionValue;
                    case "==":
                        return TimeManager.Instance.currentHour == conditionValue;
                    case ">=":
                        return TimeManager.Instance.currentHour >= conditionValue;
                    case ">":
                        return TimeManager.Instance.currentHour > conditionValue;
                    case "!=":
                        return TimeManager.Instance.currentHour != conditionValue;
                    default:
                        return false;
                }
            }

            if ((ConditionType)i.statType == conditionType)
            {
                switch (conditionOperator)
                {
                    case "<":
                        return i.currentValue < conditionValue;
                    case "<=":
                        return i.currentValue <= conditionValue;
                    case "==":
                        return i.currentValue == conditionValue;
                    case ">=":
                        return i.currentValue >= conditionValue;
                    case ">":
                        return i.currentValue > conditionValue;
                    case "!=":
                        return i.currentValue != conditionValue;
                    default:
                        return false;
                }
            }
        }
        return false;
    }

    // public bool IsConditionMet(Dictionary<ConditionType, int> dic)
    // {
    //     switch (conditionOperator)
    //     {
    //         case "<":
    //             return dic[conditionType] < conditionValue;
    //         case "<=":
    //             return dic[conditionType] <= conditionValue;
    //         case "==":
    //             return dic[conditionType] == conditionValue;
    //         case ">=":
    //             return dic[conditionType] >= conditionValue;
    //         case ">":
    //             return dic[conditionType] > conditionValue;
    //         case "!=":
    //             return dic[conditionType] != conditionValue;
    //         default:
    //             return false;
    //     }
    // }

    // public void SetTarget(Dictionary<ConditionType, int> dic)
    // {
    //     switch (targetOperator)
    //     {
    //         case "`+":
    //             dic[targetType] += targetValue;
    //             break;
    //         case "-":
    //             dic[targetType] -= targetValue;
    //             break;
    //         case "`=":
    //             dic[targetType] = targetValue;
    //             break;
    //     }
    // }

    public void SetTarget(List<Stat> stats)
    {
        foreach (var i in stats)
        {
            if ((ConditionType)i.statType == targetType)
            {
                switch (targetOperator)
                {
                    case "`+":
                        i.CurrentValue += targetValue;
                        break;
                    case "-":
                        i.CurrentValue -= targetValue;
                        break;
                    case "`=":
                        i.CurrentValue = targetValue;
                        break;
                }
                // Debug.Log($"{TAG} {targetType} {targetOperator} {targetValue}");
                string targetOperatorTemp = targetOperator == "`+" ? "+" : (targetOperator == "`=" ? "=" : "-");
                OnStatChangeRequested?.Invoke($"{conditionStrDic[targetType]} {targetOperatorTemp} {targetValue}");
            }
        }
    }

    public string GetName()
    {
        // todo 언어 설정한 뒤 그 언어로 반환
        switch (nameType)
        {
            case NameType.PRINCESS:
                return "딸";
            case NameType.MERCHANT:
                return "상인";
            case NameType.SYSTEM:
                return "시스템";
            default:
                return "무명";
        }
    }

    public string GetText()
    {
        // todo 언어 설정한 뒤 그 언어로 반환
        return korean[UnityEngine.Random.Range(0, korean.Count)];
    }
}

public static class DialogTool
{
    private static string TAG = "[DialogTool]";

    public static Dictionary<int, Dialog> dialogDic = new();
    public static Dictionary<DialogType, List<Dialog>> princessByDialogTypeDic = new();
    public static Dictionary<SituationType, List<Dialog>> princessBySituationDic = new();

    private static readonly Dictionary<DialogDataType, string> dialogFieldNames = new()
    {
        { DialogDataType.TYPE, "DialogType" },
        { DialogDataType.NAME, "Name" },
        { DialogDataType.SITUATION, "Situation" },
        { DialogDataType.CONDITION_TYPE, "ConditionType" },
        { DialogDataType.CONDITION_OPERATOR, "ConditionOperator" },
        { DialogDataType.CONDITION_VALUE, "ConditionValue" },
        { DialogDataType.EMOTION, "Emotion" },
        { DialogDataType.NEXT_ID, "NextId" },
        { DialogDataType.OPTION_ID_LIST, "OptionIdList" },
        { DialogDataType.EXT1, "Ext1" },
        { DialogDataType.TARGET_TYPE, "TargetType" },
        { DialogDataType.TARGET_OPERATOR, "TargetOperator" },
        { DialogDataType.TARGET_VALUE, "TargetValue" },
        { DialogDataType.SCHEDULE_TYPE, "ScheduleType" },
        { DialogDataType.KOREAN, "Korean" },
        { DialogDataType.ENGLISH, "English" }
    };

    public static void Init()
    {
        CsvRead("Dialog/Dialog");
        SetPrincessDialogTypeDic();
        SetPrincessDialogSituationDic();
    }

    public static void SetPrincessDialogTypeDic()
    {
        foreach (var dialog in dialogDic.Values)
        {
            if (dialog.nameType == NameType.PRINCESS)
            {
                if (!princessByDialogTypeDic.ContainsKey(dialog.type))
                {
                    princessByDialogTypeDic[dialog.type] = new List<Dialog>();
                }
                princessByDialogTypeDic[dialog.type].Add(dialog);
            }
        }
    }

    public static List<Dialog> GetDialogListByCondition(ConditionType conditionType, List<Stat> stats)
    {
        List<Dialog> dialogByConditionList = princessByDialogTypeDic[DialogType.CONDITION];
        List<Dialog> dialogList = new();
        foreach (var dialog in dialogByConditionList)
        {
            if (dialog.conditionType == conditionType && dialog.IsConditionMet(stats))
            {
                // Debug.Log($"{TAG} id {dialog.id}");
                dialogList.Add(dialog);
            }
        }
        return dialogList;
    }

    public static void SetPrincessDialogSituationDic()
    {
        foreach (var dialog in dialogDic.Values)
        {
            if (dialog.nameType == NameType.PRINCESS)
            {
                if (!princessBySituationDic.ContainsKey(dialog.situationType))
                {
                    princessBySituationDic[dialog.situationType] = new();
                }
                princessBySituationDic[dialog.situationType].Add(dialog);
            }
        }
        // foreach (var keyValue in princessBySituationDic)
        // {
        //     Debug.Log($"{TAG} princessBySituationDic {keyValue.Key} {keyValue.Value}");
        // }
        // foreach (var dialog in princessBySituationDic[SituationType.EVENING])
        // {
        //     Debug.Log($"{TAG} dialog {dialog.id} {dialog.korean} ");
        // }
    }

    public static List<Dialog> GetDialogListBySituation(SituationType situationType, List<Stat> stats)
    {
        List<Dialog> dialogSituationList = princessBySituationDic[situationType];
        List<Dialog> dialogList = new();
        foreach (var dialog in dialogSituationList)
        {
            if (dialog.situationType == situationType && dialog.IsConditionMet(stats))
            {
                dialogList.Add(dialog);
                // Debug.Log($"{TAG} dialog {dialog.id} {dialog.korean} ");
            }
        }
        return dialogList;
    }

    public static bool HasConditionType(Dialog dialog, ConditionType conditionType)
    {
        if (dialog.conditionType == conditionType) return true;
        else return false;
    }

    public static void CsvRead(string fileName)
    {
        TextAsset csvFile = Resources.Load<TextAsset>(fileName);

        using (StringReader sr = new StringReader(csvFile.text))
        {
            Dictionary<DialogDataType, int> dialogDataIndexDic = new();
            string[] langs = sr.ReadLine().Split(","); // 언어들 받아오기 Key,Id,Emotion,Korean(ko),English(en)
            for (int i = 0; i < langs.Length; i++)
            {
                foreach (var pair in dialogFieldNames)
                {
                    if (langs[i].Contains(pair.Value)) dialogDataIndexDic[pair.Key] = i;
                }
            }

            int lineNum = 2;
            string line;
            while ((line = sr.ReadLine()) != null)
            {
                string[] rows = ParseCsvLine(line); // 한 줄을 읽어서 string 배열에 넣음

                string typeStr = rows[dialogDataIndexDic[DialogDataType.TYPE]];
                string nameStr = rows[dialogDataIndexDic[DialogDataType.NAME]];
                string situationStr = rows[dialogDataIndexDic[DialogDataType.SITUATION]];
                string conditionTypeStr = rows[dialogDataIndexDic[DialogDataType.CONDITION_TYPE]];
                string conditionOperatorStr = rows[dialogDataIndexDic[DialogDataType.CONDITION_OPERATOR]];
                string conditionValueStr = rows[dialogDataIndexDic[DialogDataType.CONDITION_VALUE]];
                string emotionStr = rows[dialogDataIndexDic[DialogDataType.EMOTION]];
                string nextIdStr = rows[dialogDataIndexDic[DialogDataType.NEXT_ID]];
                string optionIdListStr = rows[dialogDataIndexDic[DialogDataType.OPTION_ID_LIST]];
                string ext1Str = rows[dialogDataIndexDic[DialogDataType.EXT1]];
                string targetTypeStr = rows[dialogDataIndexDic[DialogDataType.TARGET_TYPE]];
                string targetOperatorStr = rows[dialogDataIndexDic[DialogDataType.TARGET_OPERATOR]];
                string targetValueStr = rows[dialogDataIndexDic[DialogDataType.TARGET_VALUE]];
                string scheduleTypeStr = rows[dialogDataIndexDic[DialogDataType.SCHEDULE_TYPE]];
                List<string> korean = rows[dialogDataIndexDic[DialogDataType.KOREAN]].Split("§").ToList();
                string english = rows[dialogDataIndexDic[DialogDataType.ENGLISH]];

                DialogType type = Enum.TryParse(typeStr, out DialogType a) ? a : DialogType.NONE;
                NameType nameType = Enum.TryParse(nameStr, out NameType b) ? b : NameType.NONE;
                SituationType situationType = Enum.TryParse(situationStr, out SituationType c) ? c : SituationType.NONE;
                ConditionType conditionType = Enum.TryParse(conditionTypeStr, out ConditionType d) ? d : ConditionType.NONE;
                string conditionOperator = conditionOperatorStr == "`==" ? "==" : conditionOperatorStr;
                int conditionValue = int.TryParse(conditionValueStr, out int e) ? e : 0;
                EmotionType emotion = Enum.TryParse(emotionStr, out EmotionType f) ? f : EmotionType.NONE;
                int nextId = int.TryParse(nextIdStr, out int g) ? lineNum + g : -1;
                List<int> optionIdList = optionIdListStr.Split(',').Select(s => int.TryParse(s.Trim(), out int h) ? lineNum + h : -1).ToList();
                ExtType ext1 = Enum.TryParse(ext1Str, out ExtType i) ? i : ExtType.NONE;
                ConditionType targetType = Enum.TryParse(targetTypeStr, out ConditionType j) ? j : ConditionType.NONE;
                string targetOperator = targetOperatorStr == "`=" ? "=" : targetOperatorStr;
                int targetValue = int.TryParse(targetValueStr, out int k) ? k : 0;
                ScheduleType scheduleType = Enum.TryParse(scheduleTypeStr, out ScheduleType l) ? l : ScheduleType.NONE;

                Dialog dialog = new()
                {
                    id = lineNum,
                    type = type,
                    nameType = nameType,
                    situationType = situationType,
                    conditionType = conditionType,
                    conditionOperator = conditionOperator,
                    conditionValue = conditionValue,
                    emotion = emotion,
                    nextId = nextId,
                    optionIdList = optionIdList,
                    ext1 = ext1,
                    targetType = targetType,
                    targetOperator = targetOperator,
                    targetValue = targetValue,
                    scheduleType = scheduleType,
                    korean = korean,
                    english = english
                };

                dialogDic[lineNum++] = dialog;
            }
        }

        // 데이터 체크용
        // foreach (var entry in dic)
        // {
        //     var compositeKey = entry.Key;
        //     var dialog = entry.Value;

        //     Debug.Log($"Key: {compositeKey}");
        //     // Debug.Log($"id: {dialog.id}, type: {dialog.type}, Name: {dialog.name}, Korean: {dialog.korean}");
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
