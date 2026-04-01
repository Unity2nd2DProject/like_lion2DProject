using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class StorySceneContoller : MonoBehaviour
{
    public StoryID storyID;

    public StoryPlayerContoller player;
    public List<StoryNPCContoller> storyNPCList;
    public List<StoryInteractableObject> interactableObjects;

    public void LoadStoryDialogue()
    {
        DialogueManager.Instance.LoadStoryDialogue(storyID);

        foreach (var stroyNPC in storyNPCList)
        {
            stroyNPC.currentStoryID = storyID;
        }
    }

    public void MoveCamera(Vector2 position, float duration)
    {

    }
}
