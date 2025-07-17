using UnityEngine;

public enum MapArea
{
    Town,
    Forest,
    House1
}

public class MapZone : MonoBehaviour
{
    public MapArea areaType;
    private Collider2D col;

    void Awake()
    {
        col = GetComponent<Collider2D>();
    }

    public Bounds GetBounds()
    {
        return col.bounds;
    }
}
