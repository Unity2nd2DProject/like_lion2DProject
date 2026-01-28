using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class StorySceneContoller : MonoBehaviour
{
    public StoryID storyID;


    public StoryPlayerContoller player;
    public List<StoryNPCContoller> npcList;
    public List<StoryInteractableObject> interactableObjects;

    public void Start()
    {
        interactableObjects[0].SetInteraction(StoryInteractableObject.InteractionType.Click, () =>
        {
            Debug.Log("Object Clicked!");
        });
    }

}
