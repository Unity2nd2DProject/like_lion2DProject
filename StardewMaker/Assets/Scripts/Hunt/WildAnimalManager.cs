using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class WildAnimalManager : Singleton<WildAnimalManager>
{
    [Header("Animals")]
    [SerializeField] private Dictionary<AnimalType, int> animals = new Dictionary<AnimalType, int>();

    [Header("Items")]
    [SerializeField] private List<ItemData> rabbitDropItems = new List<ItemData>();
    [SerializeField] private List<ItemData> deerDropItems = new List<ItemData>();
    [SerializeField] private List<ItemData> wildBoarDropItems = new List<ItemData>();
    [SerializeField] private List<ItemData> bearDropItems = new List<ItemData>();

    [Header("Spawn")]
    [SerializeField] private List<GameObject> animalPrefabbs = new List<GameObject>();
    [SerializeField] private List<Transform> spawnerTransfomrs = new List<Transform>();
    [SerializeField] private float spawnTimerInterval = 10f;
    [SerializeField] private float spawnTimer;

    protected override void Awake()
    {
        base.Awake();
    }

    public void RegisterAnimal(AnimalType type)
    {
        if (animals.ContainsKey(type))
        {
            animals[type]++;
        }
        else
        {
            animals[type] = 1;
        }
        //Debug.Log($"Registered {type}. Current count: {animals[type]}");
    }

    public void UnregisterAnimal(AnimalType type)
    {
        if (animals.ContainsKey(type) && animals[type] > 0)
        {
            animals[type]--;
            //Debug.Log($"Unregistered {type}. Current count: {animals[type]}");
        }
    }

    public int GetAnimalCount(AnimalType type)
    {
        return animals.ContainsKey(type) ? animals[type] : 0;
    }

    public List<DropItem> GetDropItems(AnimalType type)
    {
        switch (type)
        {
            case AnimalType.Rabbit:
                return SetupRabbitDropItem();
            case AnimalType.Deer:
                return SetupDeerDropItem();
            case AnimalType.WildBoar:
                return SetupWildBoarDropItem();
            case AnimalType.Bear:
                return SetupBearDropItem();
            default:
                Debug.LogWarning($"No drop items defined for {type}");
                return new List<DropItem>();
        }
    }

    public List<DropItem> SetupRabbitDropItem()
    {
        var drpoItems = new List<DropItem>();
        var rabbitMeat = new DropItem(rabbitDropItems[0], 1, 1f);
        drpoItems.Add(rabbitMeat);
        return drpoItems;
    }

    public List<DropItem> SetupDeerDropItem()
    {
        var drpoItems = new List<DropItem>();
        var animalLeatehr = new DropItem(deerDropItems[0], 2, 1f);
        drpoItems.Add(animalLeatehr);
        return drpoItems;
    }

    public List<DropItem> SetupWildBoarDropItem()
    {
        var drpoItems = new List<DropItem>();
        var prok = new DropItem(wildBoarDropItems[0], 1, 1f);
        var animalLeatehr = new DropItem(wildBoarDropItems[1], 1, 1f);
        drpoItems.Add(prok);
        drpoItems.Add(animalLeatehr);
        return drpoItems;
    }

    public List<DropItem> SetupBearDropItem()
    {
        var drpoItems = new List<DropItem>();
        var bearMeat = new DropItem(bearDropItems[0], 2, 1f);
        var animalLeatehr = new DropItem(bearDropItems[1], 3, 1f);
        drpoItems.Add(bearMeat);
        drpoItems.Add(animalLeatehr);
        return drpoItems;
    }
}
