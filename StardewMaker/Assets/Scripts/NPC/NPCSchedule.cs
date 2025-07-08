using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class NPCScheduleEntry
{
    public int hour;
    public string[] routeNames;
    public NpcActionType actionOnArrival;
}

[Serializable]
public class SeasonalOverrideSchedule
{
    public Season season;
    public int day;
    public List<NPCScheduleEntry> scheduleEntries;
}

[CreateAssetMenu(fileName = "New Npc Schedule", menuName = "NPC/Create New NPC Schedule")]
public class NPCSchedule : ScriptableObject
{
    public string npcName;
    public List<NPCScheduleEntry> defaultSchedule;
    public List<SeasonalOverrideSchedule> overrideSchedules;
}
