using Unity.VisualScripting;
using UnityEngine;

public class Story01Controller : StorySceneContoller
{
    private void Start()
    {
        UIManager.Instance.FadeIn();

        interactableObjects[0].SetInteraction(StoryInteractableObject.InteractionType.Click, () =>
        {
            DialogueManager.Instance.StartStoryDialogue(storyID, 1);
        });
    }
}

