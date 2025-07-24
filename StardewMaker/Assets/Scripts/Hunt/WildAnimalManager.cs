using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    [SerializeField] private int animalCount = 3;
    [SerializeField] private GameObject rabbitPrefab;
    [SerializeField] private GameObject deerPrefab;
    [SerializeField] private GameObject wildBoarPrefab;
    [SerializeField] private GameObject bearPrefab;
    [SerializeField] private List<string> rabbitSpawnerNames = new List<string>();
    [SerializeField] private List<string> deerSpawnerNames = new List<string>();
    [SerializeField] private List<string> wildBoarSpawnerNames = new List<string>();
    [SerializeField] private List<string> bearSpawnerNames = new List<string>();
    [SerializeField] private float spawnTimerInterval = 5f;
    [SerializeField] private float spawnTimer;

    [Header("Check")]
    [SerializeField] private Dictionary<AnimalType, GameObject> animalPrefabDict;
    [SerializeField] private Dictionary<AnimalType, List<Transform>> spawnPointDict;
    [SerializeField] private List<Transform> rabbitSpawners = new List<Transform>();
    [SerializeField] private List<Transform> deerSpawners = new List<Transform>();
    [SerializeField] private List<Transform> wildBoarSpawners = new List<Transform>();
    [SerializeField] private List<Transform> bearSpawners = new List<Transform>();

    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        //SetSpawners();
    }

    public void SetSpawners()
    {
        rabbitSpawners = FindSpawnersByName(rabbitSpawnerNames);
        deerSpawners = FindSpawnersByName(deerSpawnerNames);
        wildBoarSpawners = FindSpawnersByName(wildBoarSpawnerNames);
        bearSpawners = FindSpawnersByName(bearSpawnerNames);

        animalPrefabDict = new Dictionary<AnimalType, GameObject>
        {
            { AnimalType.Rabbit, rabbitPrefab },
            { AnimalType.Deer, deerPrefab },
            { AnimalType.WildBoar, wildBoarPrefab },
            { AnimalType.Bear, bearPrefab }
        };

        spawnPointDict = new Dictionary<AnimalType, List<Transform>>
        {
            { AnimalType.Rabbit, rabbitSpawners },
            { AnimalType.Deer, deerSpawners },
            { AnimalType.WildBoar, wildBoarSpawners },
            { AnimalType.Bear, bearSpawners }
        };
    }

    private List<Transform> FindSpawnersByName(List<string> spawnerNames)
    {
        var found = new List<Transform>();

        foreach (string name in spawnerNames)
        {
            var wp = WaypointManager.Instance.GetPosition(name);
            if (wp != null)
            {
                found.Add(wp);
            }
            else
            {
                Debug.LogWarning($"Spawner not found: {name}");
            }
        }

        return found;
    }

    private void Update()
    {
        if (SceneManager.GetActiveScene().name.Contains("Home"))
        {
            return;
        }

        spawnTimer += Time.deltaTime;
        if (spawnTimer >= spawnTimerInterval)
        {
            TrySpawnAllAnimals();
            spawnTimer = 0f;
        }
    }

    private void TrySpawnAllAnimals()
    {
        foreach (AnimalType type in System.Enum.GetValues(typeof(AnimalType)))
        {
            int count = GetAnimalCount(type);
            if (count < animalCount)
            {
                SpawnAnimal(type);
            }
        }
    }

    public void SpawnAnimal(AnimalType type)
    {
        if (!animalPrefabDict.ContainsKey(type) || !spawnPointDict.ContainsKey(type))
        {
            return;
        }

        var prefab = animalPrefabDict[type];
        var spawners = spawnPointDict[type];
        if (spawners.Count == 0)
        {
            return;
        }

        var spawnPoint = spawners[UnityEngine.Random.Range(0, spawners.Count)];
        Instantiate(prefab, spawnPoint.position, Quaternion.identity);
        RegisterAnimal(type);
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
        Debug.Log($"Registered {type}. Current count: {animals[type]}");
    }

    public void UnregisterAnimal(AnimalType type)
    {
        if (animals.ContainsKey(type) && animals[type] > 0)
        {
            animals[type]--;
            Debug.Log($"Unregistered {type}. Current count: {animals[type]}");
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
