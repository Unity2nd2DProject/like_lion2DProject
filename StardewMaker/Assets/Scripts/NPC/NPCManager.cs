using System.Collections.Generic;
using UnityEngine;

public class NPCManager : Singleton<NPCManager>
{
    [Header("NPC")]
    [SerializeField] private List<GameObject> npcPrefabs;

    [Header("Check")]
    [SerializeField] private List<NPCController> activeNPCs = new List<NPCController>();

    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        SaveManager.Instance.LoadNPC();
    }

    public void SpawnNPCs()
    {
        ClearAllNPCs();

        foreach (var prefab in npcPrefabs)
        {
            GameObject npcObj = Instantiate(prefab);
            NPCController npc = npcObj.GetComponent<NPCController>();

            if (npc != null)
            {
                activeNPCs.Add(npc);
            }
        }
    }

    private void ClearAllNPCs()
    {
        foreach (var npc in activeNPCs)
        {
            if (npc != null)
            {
                Destroy(npc.gameObject);
            }
        }
        activeNPCs.Clear();
    }

    public void OnTimeChanged(int hour, int minute)
    {
        foreach (var npc in activeNPCs)
        {
            npc.OnTimeChanged(hour, minute);
        }
    }

    public void NextDay()
    {
        foreach (var npc in activeNPCs)
        {
            npc.ResetToDefaultPosition();
        }
    }

    public NPCSaveData SaveNPCs()
    {
        NPCSaveData data = new NPCSaveData();

        foreach (var npc in activeNPCs)
        {
            var mover = npc.GetComponent<NPCMover>();
            data.savedNPCs.Add(new SavedNPC
            {
                npcName = npc.npcName,
                position = npc.transform.position,
                currentAction = mover != null ? mover.GetCurrentAction() : NpcActionType.None,
                routeIndex = mover != null ? mover.GetCurrentRouteIndex() : 0,
                teleportTarget = mover != null ? mover.GetTeleportTarget() : null
            });
        }

        return data;
    }

    public void LoadNPCs(NPCSaveData data)
    {
        ClearAllNPCs();
        SpawnNPCs();

        foreach (var saved in data.savedNPCs)
        {
            var npc = activeNPCs.Find(x => x.npcName == saved.npcName);
            if (npc != null)
            {
                npc.transform.position = saved.position;
                var mover = npc.GetComponent<NPCMover>();
                if (mover != null)
                {
                    mover.RestoreState(saved.currentAction, saved.routeIndex, saved.teleportTarget);
                }
            }
        }
    }

}
