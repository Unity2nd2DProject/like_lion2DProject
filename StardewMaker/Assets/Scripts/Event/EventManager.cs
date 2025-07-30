using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : Singleton<EventManager>
{
    [Header("Events")]
    [SerializeField] private List<EventData> eventDatas;

    private List<EventInstance> activeEvents = new List<EventInstance>();

    protected override void Awake()
    {
        base.Awake();
    }
}
