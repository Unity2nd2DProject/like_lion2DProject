using TMPro;
using UnityEngine;

public class QuestSlotUI : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private TextMeshProUGUI questNameText;
    [SerializeField] private TextMeshProUGUI isCompletedText;
    //[SerializeField] private GameObject questDetailPopup;

    public void Setup(string questName, bool isCompleted)
    {
        questNameText.text = questName;
        isCompletedText.text = isCompleted ? "(완료)" : "(진행중)";
    }
}
