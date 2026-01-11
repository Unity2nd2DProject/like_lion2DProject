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
        SaveLoadManager.Instance.LoadNPC();
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

    // TODO: 시간 변화에 따른 NPC 행동 업데이트 -> 다른 방식으로 바꿔여 할듯.
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
            npc.mover.ResetToDefaultPosition();
        }
    }

    // Save / Load NPCs NPC매니저에서 하는것은 맞으나 조금 더 다른 방식으로 해야할듯. 
    public NPCSaveData SaveNPCs()
    {
        NPCSaveData data = new NPCSaveData();

        foreach (var npc in activeNPCs)
        {
            var mover = npc.GetComponent<NPCMove>();
            data.savedNPCs.Add(new SavedNPC
            {
                npcId = npc.npcID,
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
            var npc = activeNPCs.Find(x => x.npcID == saved.npcId);
            if (npc != null)
            {
                npc.transform.position = saved.position;
                var mover = npc.GetComponent<NPCMove>();
                if (mover != null)
                {
                    mover.RestoreState(saved.currentAction, saved.routeIndex, saved.teleportTarget);
                }
            }
        }
    }

    public int GetFrindShip(NPC.NpcId npcId)
    {
        return activeNPCs.Find(x => x.npcID == npcId).friendshipPoints;
    }

    public void AddFriendShipPoint(NPC.NpcId npcId, int amount)
    {
        activeNPCs.Find(x => x.npcID == npcId).friendshipPoints += amount;
    }

}
