using System;
using System.Reflection;
using UnityEngine;


namespace NPC
{
    public enum NpcId
    {
        None,
        Sera,
        Barun,

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
        Shop,
        ShopEnd,
        QuestOffer,
        QuestCompletion,
        QuestDecline,
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
