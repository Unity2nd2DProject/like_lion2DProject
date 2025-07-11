using System.Collections.Generic;
using UnityEngine;

public class MapManager : Singleton<MapManager>
{
    private List<MapZone> zones;

    protected override void Awake()
    {
        zones = new List<MapZone>(FindObjectsOfType<MapZone>());
    }

    public MapArea GetArea(Vector2 position)
    {
        foreach (var zone in zones)
        {
            if (zone.GetBounds().Contains(position))
                return zone.areaType;
        }
        return MapArea.Town;
    }
}
