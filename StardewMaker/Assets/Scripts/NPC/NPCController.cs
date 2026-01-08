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

    // Components
    [HideInInspector] public NPCMove mover;
    [HideInInspector] public NPCQuestGiver questGiver;
    [HideInInspector] public NPCVendor vendor;

    [SerializeField] private float interactionRange = 2.5f;

    public bool shopAvailable = false;
    public bool questAvailable = false;

    private void Awake()
    {
        mover = GetComponent<NPCMove>();

        if (TryGetComponent(out vendor))
        {
            shopAvailable = true;
        }

        if (TryGetComponent(out questGiver))
        {
            questAvailable = true;
        }
    }
    private void Start()
    {
        // transform.position = WaypointManager.Instance.GetPosition(defaultPosition).position;
        OnTimeChanged(TimeManager.Instance.currentHour, TimeManager.Instance.currentMinute); // ?
    }

    public void OnTimeChanged(int hour, int minute) 
    {
        OnHourChanged(TimeManager.Instance.currentHour);
    }
    

    private void OnMouseDown()
    {
        Debug.Log($"[NPCInteraction] {npcID} 클릭됨");
        Transform playerTransform = PlayerController.Instance.transform;
        float distance = Vector2.Distance(transform.position, playerTransform.position);

        if (distance > interactionRange)
        {
            Debug.Log($"[NPCInteraction] to far to interact");
            return;
        }
        else if (UIManager.Instance.IsUIOn())
        {
            Debug.Log($"[NPCInteraction] UI is on, cannot interact");
            return;
        }
        else
        {
            DialogueManager.Instance.StartDialogue(this);
        }
    }

    // 시간이 지나면 동작이지만 아워체인지와 타임체인지가 나눠진 이유;
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
}
