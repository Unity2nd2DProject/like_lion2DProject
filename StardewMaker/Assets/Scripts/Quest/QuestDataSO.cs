using NPC;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public enum QuestType
{
    Tutorial,
    DailyQuest,
    Quest
}

#region Reward관련

public enum RewardType
{
    Item,
    Money,
    FriendshipPoint
}

[Serializable]
public class Reward
{
    public RewardType rewardType;

    public ItemData item;
    public int itemQuantity;

    public int money;

    public NpcId npc;
    public int friendshipPoint;
}

[CustomPropertyDrawer(typeof(Reward))]
public class RewardDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        float lineHeight = EditorGUIUtility.singleLineHeight + 2;
        Rect r = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);

        // rewardType
        SerializedProperty rewardTypeProp = property.FindPropertyRelative("rewardType");
        EditorGUI.PropertyField(r, rewardTypeProp);
        r.y += lineHeight;

        RewardType type = (RewardType)rewardTypeProp.enumValueIndex;

        // 타입별 필드 표시
        switch (type)
        {
            case RewardType.Item:
                EditorGUI.PropertyField(r, property.FindPropertyRelative("item"));
                r.y += lineHeight;
                EditorGUI.PropertyField(r, property.FindPropertyRelative("itemQuantity"));
                break;

            case RewardType.Money:
                EditorGUI.PropertyField(r, property.FindPropertyRelative("money"));
                break;

            case RewardType.FriendshipPoint:
                EditorGUI.PropertyField(r, property.FindPropertyRelative("npc"));
                r.y += lineHeight;
                EditorGUI.PropertyField(r, property.FindPropertyRelative("friendshipPoint"));
                break;
        }

        EditorGUI.EndProperty();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        RewardType type = (RewardType)property.FindPropertyRelative("rewardType").enumValueIndex;
        float lineHeight = EditorGUIUtility.singleLineHeight + 2;

        switch (type)
        {
            case RewardType.Item: return lineHeight * 3;
            case RewardType.Money: return lineHeight * 2;
            case RewardType.FriendshipPoint: return lineHeight * 3;
        }

        return lineHeight;
    }
}

#endregion

[CreateAssetMenu(fileName = "New Quest", menuName = "Quest/Create New Quest")]
public class QuestDataSO : ScriptableObject
{
    [Header("Info")]
    public string questID;
    public QuestType questType;
    public string questName;
    [TextArea] public string description;
    public string goalDescrpition;

    [Header("Goal")]
    public List<QuestGoal> goals = new List<QuestGoal>();

    [Header("Reward")]
    public List<Reward> rewards = new();

    [Header("Condition")]
    public int unlockAfterDay;
}
