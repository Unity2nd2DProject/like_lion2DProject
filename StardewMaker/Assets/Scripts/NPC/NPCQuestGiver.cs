using System.Collections.Generic;
using UnityEngine;

public class NPCQuestGiver : MonoBehaviour
{
    public QuestPool questPool;
    public QuestDataSO todaysQuest;
    public bool isQuestGiven = false;

    void Start()
    {
        SetTodaysQuest();
    }
    public void SetTodaysQuest()
    {
        todaysQuest = questPool.GetRandomAvailableQuest();
    }

    public void AcceptQuest()
    {
        QuestManager.Instance.AcceptQuest(todaysQuest.questID, questPool.npcId);
        isQuestGiven = true;
    }
}
