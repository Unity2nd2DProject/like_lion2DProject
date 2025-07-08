using System.Collections.Generic;
using UnityEngine;

public class NPCManager : Singleton<NPCManager>
{
    [Header("npcs")]
    [SerializeField] private List<NPCController> npcs = new List<NPCController>();

    protected override void Awake()
    {
        base.Awake();
    }

    public void Register(NPCController npc)
    {
        if (!npcs.Contains(npc))
        {
            npcs.Add(npc);
        }
    }

    public void Unregister(NPCController npc)
    {
        npcs.Remove(npc);
    }

    public void OnHourChanged(int hour)
    {
        foreach (var npc in npcs)
        {
            npc.OnHourChanged(hour);
        }
    }
}
