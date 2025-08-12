using System.Collections.Generic;
using UnityEngine;

public class MapManager : Singleton<MapManager>
{
    private List<MapZone> zones;

    protected override void Awake()
    {
        zones = new List<MapZone>(FindObjectsOfType<MapZone>());
        
        foreach(var zone in zones)
        {
            //Debug.Log($"[MapManager] {zone.areaType} : {zone.GetBounds()}");
        }
    }

    public MapArea GetArea(Vector2 position)
    {
        //Debug.Log($"[MapManager] player : {position}");
        foreach (var zone in zones)
        {
            //if (zone.GetBounds().Contains(position))
            //{
            //    return zone.areaType;
            //}
            var bounds = zone.GetBounds();
            if (position.x >= bounds.min.x && position.x <= bounds.max.x &&
                position.y >= bounds.min.y && position.y <= bounds.max.y)
            {
                return zone.areaType;
            }
        }
        return MapArea.Town;
    }
}
