using UnityEngine;

public enum EventCategory
{
    Random,
    Periodic,
    Statebased,
    Special
}

public class EventData : MonoBehaviour
{
    [Header("Info")]
    public string eventName;
    [TextArea] public string description;
    public EventCategory category;
    [TextArea] public string conditionHint; // 이벤트 발동 조건

}
