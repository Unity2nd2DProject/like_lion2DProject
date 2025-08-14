using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : Singleton<EventManager>
{
    [Header("Events")]
    [SerializeField] private List<EventData> events;

    private List<int> activeEvents = new List<int>();

    protected override void Awake()
    {
        base.Awake();
    }

    public void TriggerEvents()
    {
        TriggerRandomEvents();
        TriggerDateEvents();
        TriggerStateBasedEvents();
    }

    private void TriggerRandomEvents()
    {
        foreach (var e in events)
        {
            if (e.eventType != EventType.Random)
            {
                continue;
            }

            if (Random.value <= e.triggerChance)
            {
                TriggerEvent(e);
                break; // 하루에 하나만
            }
        }
    }

    private void TriggerDateEvents()
    {
        var tm = TimeManager.Instance;

        foreach (var e in events)
        {
            if (e.eventType != EventType.Date) continue;

            bool isCorrectSeason = e.season == tm.currentSeason;
            bool isCorrectDay = e.day == tm.currentDay;
            bool isCorrectYear = (e.year == -1 || e.year == tm.currentYear);

            if (isCorrectSeason && isCorrectDay && isCorrectYear)
            {
                TriggerEvent(e);
            }
        }
    }

    private void TriggerStateBasedEvents()
    {

    }

    private void TriggerEvent(EventData eventData)
    {
        // TODO
        Debug.Log($"========== [Event] 이벤트 발생! {eventData.eventName} ==========");
        activeEvents.Add(eventData.eventId);
    }

    public void DeactiveEvents()
    {
        activeEvents.Clear();
    }
}
