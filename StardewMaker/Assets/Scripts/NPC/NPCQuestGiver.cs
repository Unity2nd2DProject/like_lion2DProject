using System.Collections.Generic;
using UnityEngine;

public class NPCQuestGiver : MonoBehaviour
{
    public QuestPool questPool;
    public QuestDataSO todaysQuest;
    public bool isQuestGiven = false;

    public QuestDataSO GetAvailableQuest()
    {
        QuestDataSO availableQuest = questPool.GetRandomAvailableQuest();

        if (availableQuest != null)
        {
            return availableQuest;
        }
        else
        {
            return availableQuest;
        }
    }

    public void SetTodaysQuest()
    {
        todaysQuest = questPool.GetRandomAvailableQuest();
    }

    public void GiveQuest()
    {


        if (todaysQuest != null)
        {
            if (!QuestManager.Instance.IsQuestActive(todaysQuest.questID))
            {
                QuestManager.Instance.AcceptQuest(todaysQuest.questID);
                isQuestGiven = true;
                Debug.Log($"[NPCQuestGiver] {todaysQuest.questName} 퀘스트 수락!");
            }
            else
            {
                Debug.Log("[NPCQuestGiver] 이미 수락한 퀘스트입니다");
            }
        }
        else
        {
            Debug.Log("[NPCQuestGiver] 지금은 퀘스트를 받을 수 없습니다.");
        }
    }
}
