using System;
using UnityEngine;

public class DialogTrigger : MonoBehaviour
{
    private string TAG = "[DialogTrigger]";

    private UserInputManager inputManager;

    public static event Action<int> OnDialogRequested;

    private NPC currentNPC;

    void OnEnable()
    {
        inputManager = UserInputManager.Instance;
    }

    void Update()
    {
        if (inputManager.inputActions.UI.C.WasPressedThisFrame() && currentNPC != null)
        {
            OnDialogRequested?.Invoke(currentNPC.dialogId); //, currentNPC.GetPortrait());
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("NPC"))
        {
            currentNPC = other.GetComponent<NPC>();
            Debug.Log($"{TAG} NPC meet, NPC dialogId : {currentNPC.dialogId}");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("NPC"))
        {
            Debug.Log($"{TAG} NPC bye");
            currentNPC = null;
        }
    }
}
