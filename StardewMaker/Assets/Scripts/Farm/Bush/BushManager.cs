using System.Collections.Generic;
using UnityEngine;

public class BushManager : Singleton<BushManager>
{
    public GameObject redFruitBushPrefab;
    public GameObject yellowFruitBushPrefab;
    public List<Bush> bushes;

    protected override void Awake()
    {
        base.Awake();
    }

    public void NextDay()
    {
        foreach (var bush in bushes)
        {
            bush.NextDay();
        }
    }

    public List<SavedBush> SaveBushes()
    {
        List<SavedBush> list = new List<SavedBush>();
        foreach (var bush in bushes)
        {
            var (_fruitType, _hasFruit) = bush.GetState();
            list.Add(new SavedBush
            {
                position = bush.transform.position,
                fruitType = _fruitType,
                hasFruit = _hasFruit
            });
        }
        return list;
    }

    public List<SavedBush> NextDayBushes(List<SavedBush> savedList)
    {
        List<SavedBush> list = new List<SavedBush>();

        foreach (var saved in savedList)
        {
            list.Add(new SavedBush
            {
                position = saved.position,
                fruitType = saved.fruitType,
                hasFruit = true,
            });
        }
        return list;
    }

    public void LoadBushes(List<SavedBush> savedBushes)
    {
        bushes = new List<Bush>();

        foreach (var data in savedBushes)
        {
            GameObject prefab;
            if (data.fruitType == FruitType.Yellow)
            {
                prefab = yellowFruitBushPrefab;
            }
            else
            {
                prefab = redFruitBushPrefab;
            }
            //GameObject prefab = Random.value < 0.5f ? yellowFruitBushPrefab : redFruitBushPrefab;
            GameObject obj = Instantiate(prefab, data.position, Quaternion.identity);

            Bush bush = obj.GetComponent<Bush>();
            bush.SetState(data.fruitType, data.hasFruit);
            bushes.Add(bush);
        }
    }
}
