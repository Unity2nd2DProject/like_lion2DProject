using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : Singleton<DialogueManager>
{
    [SerializeField] private GameObject dialogUI;

    [SerializeField] private TypewriterEffect typewriterEffect;
    [SerializeField] private TextMeshProUGUI dialogueText;

    private bool hasShownButtons = false;

    protected override void Awake()
    {
        base.Awake();
    }

}

