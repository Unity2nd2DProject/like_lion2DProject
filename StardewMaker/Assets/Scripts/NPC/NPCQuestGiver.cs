using System.Collections.Generic;
using UnityEngine;

public class NPCQuestGiver : MonoBehaviour
{
    public QuestPool questPool;

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

    public void GiveQuest()
    {
        QuestDataSO availableQuest = questPool.GetRandomAvailableQuest();

        if (availableQuest != null)
        {
            if (!QuestManager.Instance.IsQuestActive(availableQuest.questID))
            {
                QuestManager.Instance.AcceptQuest(availableQuest.questID);
                Debug.Log($"[NPCQuestGiver] {availableQuest.questName} 퀘스트 수락!");
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
