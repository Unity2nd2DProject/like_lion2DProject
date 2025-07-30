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

    public void OnHourChanged(int hour)
    {
        foreach (var npc in activeNPCs)
        {
            npc.OnHourChanged(hour);
        }
    }

    public void NextDay()
    {
        foreach (var npc in activeNPCs)
        {
            npc.ResetToDefaultPosition();
        }
    }
}
