using System;
using UnityEngine;

public class StoryInteractableObject : MonoBehaviour
{

    public enum InteractionType
    {
        Enter,
        Click,
    }

    public Collider2D objectCollider;

    public InteractionType interactionType;

    private Action onInteract;

    private void Start()
    { 

    }

    private void OnMouseDown()
    {
        if(interactionType == InteractionType.Click)
            onInteract?.Invoke();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (interactionType == InteractionType.Enter)
            onInteract?.Invoke();
    }

    public void SetInteraction(InteractionType it, Action action)
    {
        interactionType = it;
        onInteract = action;
    }



}
