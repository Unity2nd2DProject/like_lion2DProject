using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public enum NpcActionType
{
    None,
    Idle,
    Walk,
    Fish,
    Chat
}

public class NPCController : MonoBehaviour
{
    public string npcName;
    public NPCSchedule schedule;

    private NPCMover mover;

    private void Awake()
    {
        mover = GetComponent<NPCMover>();
    }

    private void Start()
    {
        OnHourChanged(TimeManager.Instance.currentHour);
    }

    public void OnHourChanged(int hour)
    {
        var entries = GetTodayScheduleEntries();
        if (entries == null)
        {
            return;
        }

        foreach (var entry in entries)
        {
            if (entry.hour == hour)
            {
                var route = ResolveRouteFromNames(entry.routeNames);
                mover.SetRoute(route, entry.actionOnArrival);
                break;
            }
        }
    }

    private List<NPCScheduleEntry> GetTodayScheduleEntries()
    {

        foreach (var special in schedule.overrideSchedules)
        {
            if (special.season == TimeManager.Instance.currentSeason &&
                special.day == TimeManager.Instance.currentDay)
            {
                return special.scheduleEntries;
            }
        }

        return schedule.defaultSchedule;
    }

    private Transform[] ResolveRouteFromNames(string[] names)
    {
        List<Transform> list = new List<Transform>();
        foreach (string name in names)
        {
            GameObject go = GameObject.Find(name);
            if (go != null)
            {
                list.Add(go.transform);
            }
            else
            {
                Debug.LogWarning($"❌ Waypoint '{name}' not found in scene.");
            }
        }
        return list.ToArray();
    }

}
