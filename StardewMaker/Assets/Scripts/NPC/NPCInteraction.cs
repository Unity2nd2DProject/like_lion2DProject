using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class NPCInteraction : MonoBehaviour
{
    private NPCQuestGiver questGiver;

    private void Awake()
    {
        questGiver = GetComponent<NPCQuestGiver>();
    }

    private void OnMouseDown()
    {
        questGiver.GiveQuest();
    }
}
