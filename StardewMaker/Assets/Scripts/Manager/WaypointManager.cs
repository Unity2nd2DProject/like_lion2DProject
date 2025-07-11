using System.Collections.Generic;
using UnityEngine;

public class WaypointManager : Singleton<WaypointManager>
{

    [SerializeField] private Dictionary<string, Transform> waypoints = new Dictionary<string, Transform>();

    protected override void Awake()
    {
        base.Awake();

        foreach (var marker in FindObjectsOfType<Waypoint>())
        {
            waypoints[marker.id] = marker.transform;
        }

        //foreach (var wp in waypoints)
        //{
        //    Debug.Log($"{wp.Key} : {wp.Value}");
        //}
    }

    public Transform GetPosition(string id)
    {
        return waypoints.TryGetValue(id, out var t) ? t : null;
    }
}
