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
    public NPC.NpcId npcID;
    public NPCSchedule schedule;

    [Header("Movement")]
    public string defaultPosition;
    private NPCMover mover;

    [SerializeField] private float interactionRange = 2.5f;

    public bool shopAvailable;
    public bool questAvailable;

    private void Awake()
    {
        mover = GetComponent<NPCMover>();
    }

    private void Start()
    {
        transform.position = WaypointManager.Instance.GetPosition(defaultPosition).position;
        OnHourChanged(TimeManager.Instance.currentHour);
    }

    private void OnMouseDown()
    {
        Debug.Log($"[NPCInteraction] {npcID} 클릭됨");
        Transform playerTransform = PlayerController.Instance.transform;
        float distance = Vector2.Distance(transform.position, playerTransform.position);

        if (distance > interactionRange)
        {
            return;
        }
        else
        {
            DialogueManager.Instance.StartDialogue(this);
        }
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
