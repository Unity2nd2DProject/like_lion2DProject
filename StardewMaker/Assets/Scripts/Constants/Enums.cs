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

    public enum DialogueSequenceType // 대사 시퀀스 즉, 대화의 종류
    {
        Greeting,
        Chat,
        Shop,
        ShopEnd,
        QuestOffer,
        QuestCompletion,
        QuestDecline,
        QuestUnavailable,
        Farewell,
        Custom
    }

    public enum DialogueSequenceTag
    {

    }
}

public enum Season
{
    Spring,
    Summer,
    Fall,
    Winter
}
