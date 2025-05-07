using System.Collections.Generic;
using UnityEngine;

public class FarmLandManager : Singleton<FarmLandManager>
{
    public GameObject farmlandPrefab;

    public Vector2Int topLeft;
    public Vector2Int bottomRight;

    private Dictionary<Vector2, FarmLand> farmLands = new Dictionary<Vector2, FarmLand>();

    protected override void Awake()
    {
        base.Awake();

        //GenerateFarmLands();
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

    public List<SavedFarmLand> SaveFarmLands()
    {
        List<SavedFarmLand> list = new List<SavedFarmLand>();
        foreach (var kvp in farmLands)
        {
            list.Add(new SavedFarmLand
            {
                position = kvp.Key,
                landState = kvp.Value.landState
            });
        }
        return list;
    }

    public List<SavedFarmLand> NextDayFarmLands(List<SavedFarmLand> savedList)
    {
        List<SavedFarmLand> list = new List<SavedFarmLand>();

        foreach (var saved in savedList)
        {
            LandState newState = saved.landState;
            if (newState == LandState.Watered)
            {
                newState = LandState.Fertile;
            }

            list.Add(new SavedFarmLand
            {
                position = saved.position,
                landState = newState
            });
        }
        return list;
    }

    public void LoadFarmLands(List<SavedFarmLand> savedList)
    {
        foreach (var saved in savedList)
        {
            GameObject obj = Instantiate(farmlandPrefab, saved.position, Quaternion.identity);
            FarmLand land = obj.GetComponent<FarmLand>();
            land.landState = saved.landState;
            farmLands[saved.position] = land;
            farmLands[saved.position].UpdateTileSprite();
        }
    }

    public void RegisterFarmLand(FarmLand land)
    {
        Vector2 pos = land.GetPosition();
        if (!farmLands.ContainsKey(pos))
        {
            farmLands[pos] = land;
        }
    }
}
