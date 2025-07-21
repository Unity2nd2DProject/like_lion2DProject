using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum NpcActionType
{
    None,
    Teleport,
    Idle,
    Walk,
    Fish,
    Chat,
}

public class NPCController : MonoBehaviour
{
    public string npcName;
    public NPCSchedule schedule;
    private NPCMover mover;

    [Header("position")]
    public string defaultPosition;

    private void Awake()
    {
        mover = GetComponent<NPCMover>();
    }

    private void Start()
    {
        transform.position = WaypointManager.Instance.GetPosition(defaultPosition).position;
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
                var route = ResolveRouteFromId(entry.routes);
                if (entry.teleportTarget != null)
                {
                    mover.SetRoute(route, entry.actionOnArrival, entry.teleportTarget);
                }
                else
                {
                    mover.SetRoute(route, entry.actionOnArrival);
                }
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

    private Transform[] ResolveRouteFromId(string[] ids)
    {
        List<Transform> list = new List<Transform>();
        foreach (string id in ids)
        {
            list.Add(WaypointManager.Instance.GetPosition(id));
        }
        return list.ToArray();
    }

    public void ResetToDefaultPosition()
    {
        transform.position = WaypointManager.Instance.GetPosition(defaultPosition).position;
    }
}
