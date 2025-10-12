using System;
using System.Reflection;
using UnityEngine;


namespace NPC
{
    public enum NpcId
    {
        None,
        Sera,
    }

    public enum NpcEmotion
    {
        Neutral,
        Happy,
        Sad,
        Surprised
    }

    public enum DialogueSequenceType
    {
        Greeting,
        Chat,
        QuestOffer,
        QuestCompletion,
        Farewell,
        Custom
    }
}

public enum Season
{
    Spring,
    Summer,
    Fall,
    Winter
}
