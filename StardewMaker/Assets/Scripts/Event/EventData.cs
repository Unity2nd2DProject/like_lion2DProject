using UnityEngine;

public enum EventType
{
    Random,
    Date,
    Statebased,
    Special
}

[CreateAssetMenu(fileName = "New Event", menuName = "Event/Create New Event")]

public class EventData : ScriptableObject
{
    [Header("Info")]
    public int eventId;
    public string eventName;
    public EventType eventType;
    [TextArea] public string description;
    [TextArea] public string effect;

    [Header("Debug")]
    [TextArea] public string conditionHint;

    [Header("Condition (Random)")]
    public float triggerChance = 0.05f;

    [Header("Condition (Date)")]
    public Season season;
    public int day;
    public int year = -1; // -1은 매년 발생
}
