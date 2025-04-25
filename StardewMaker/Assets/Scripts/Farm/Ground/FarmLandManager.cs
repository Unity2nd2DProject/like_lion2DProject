using System.Collections.Generic;
using UnityEngine;

public class FarmLandManager : MonoBehaviour
{
    public static FarmLandManager Instance;
    public GameObject farmlandPrefab;

    public Vector2Int topLeft;
    public Vector2Int bottomRight;

    private Dictionary<Vector2, FarmLand> farmLands = new Dictionary<Vector2, FarmLand>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        GenerateFarmLands();
    }

    public void GenerateFarmLands()
    {
        for (int y=topLeft.y; y>=bottomRight.y; y--)
        {
            for(int x= topLeft.x; x<=bottomRight.x; x++)
            {
                Vector2Int pos = new Vector2Int(x, y);

                if (farmLands.ContainsKey(pos))
                {
                    continue;
                }

                //GameObject farmLandObj = Instantiate(farmlandPrefab, new Vector3(x, y, 0), Quaternion.identity, transform);
                GameObject farmLandObj = Instantiate(farmlandPrefab, new Vector3(x + 0.5f, y + 0.5f, 0), Quaternion.identity);
                FarmLand farmLand = farmLandObj.GetComponent<FarmLand>();

                farmLands.Add(new Vector2(x + 0.5f, y + 0.5f), farmLand);
            }
        }
    }

    public void NextDay()
    {
        foreach (var farmLand in farmLands.Values)
        {
            farmLand.NextDay();
        }
    }

    public FarmLand GetFarmLandAt(Vector2Int position)
    {
        if (farmLands.TryGetValue(position, out FarmLand land))
        {
            return land;
        }
        else
        {
            return null;
        }
    }
}
