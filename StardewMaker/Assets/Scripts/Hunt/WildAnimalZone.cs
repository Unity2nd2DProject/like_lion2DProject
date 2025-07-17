using UnityEngine;

public enum AnimalZoneArea
{
    Rabbit,
    Deer,
    WildBoar,
    Bear
}

public class WildAnimalZone : MonoBehaviour
{
    public AnimalZoneArea areaType;
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
