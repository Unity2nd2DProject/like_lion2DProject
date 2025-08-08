using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveManager : Singleton<SaveManager>
{
    public float autoSaveInterval = 30f;

    private string farmPath => Application.persistentDataPath + "/Save/farm.json"; // Application.dataPath
    private string baseDataPath => Application.persistentDataPath + "/Save/baseData.json";
    private string inventroyPath => Application.persistentDataPath + "/Save/inventory.json";
    private string statsPath => Application.persistentDataPath + "/Save/stats.json";

    private void Start()
    {
        //StartCoroutine(AutoSaveRoutine());
    }

    public void SaveFarm()
    {
        FarmData data = new FarmData
        {
            savedFarmLands = FarmLandManager.Instance.SaveFarmLands(),
            savedCrops = CropManager.Instance.SaveCrops(),
            savedTrees = TreeManager.Instance.SaveTrees(),
            savedBushes = BushManager.Instance.SaveBushes()
        };

        string json = JsonUtility.ToJson(data, true);
        System.IO.File.WriteAllText(farmPath, json);
        Debug.Log("농장 정보가 저장되었습니다. " + farmPath);
    }

    public void NextDayFarm()
    {
        string json = System.IO.File.ReadAllText(farmPath);
        FarmData data = JsonUtility.FromJson<FarmData>(json);

        data = new FarmData
        {
            savedFarmLands = FarmLandManager.Instance.NextDayFarmLands(data.savedFarmLands),
            savedCrops = CropManager.Instance.NextDayCrops(data.savedCrops),
            savedTrees = TreeManager.Instance.NextDayTrees(data.savedTrees),
            savedBushes = BushManager.Instance.NextDayBushes(data.savedBushes)    
        };

        json = JsonUtility.ToJson(data, true);
        System.IO.File.WriteAllText(farmPath, json);
        Debug.Log("농장 정보가 업데이트되었습니다. " + farmPath);
    }

    public void LoadFarm()
    {
        if (!System.IO.File.Exists(farmPath))
        {
            Debug.LogWarning("save.json 파일이 존재하지 않습니다.");
            return;
        }

        string json = System.IO.File.ReadAllText(farmPath);
        FarmData data = JsonUtility.FromJson<FarmData>(json);

        FarmLandManager.Instance.LoadFarmLands(data.savedFarmLands);
        CropManager.Instance.LoadCrops(data.savedCrops);
        TreeManager.Instance.LoadTrees(data.savedTrees);
        BushManager.Instance.LoadBushes(data.savedBushes);
    }

    public void SaveBase()
    {
        GameBaseData dateTime = new GameBaseData
        {
            year = TimeManager.Instance.currentYear,
            season = (int)TimeManager.Instance.currentSeason,
            day = TimeManager.Instance.currentDay,
            hour = TimeManager.Instance.currentHour,
            minute = TimeManager.Instance.currentMinute,
            staminaStates = StaminaManager.Instance.staminaStates,
            money = InventoryManager.Instance.PlayerMoney
        };

        string json = JsonUtility.ToJson(dateTime, true);
        System.IO.File.WriteAllText(baseDataPath, json);
        Debug.Log("기본 정보가 저장되었습니다. " + baseDataPath);
    }

    public void LoadTime()
    {
        if (!System.IO.File.Exists(baseDataPath))
        {
            Debug.LogWarning("baseData.json 파일이 존재하지 않습니다.");
            return;
        }

        string json = System.IO.File.ReadAllText(baseDataPath);
        GameBaseData baseData = JsonUtility.FromJson<GameBaseData>(json);

        TimeManager.Instance.currentYear = baseData.year;
        TimeManager.Instance.currentSeason = (Season)baseData.season;
        TimeManager.Instance.currentDay = baseData.day;
        TimeManager.Instance.currentHour = baseData.hour;
        TimeManager.Instance.currentMinute = baseData.minute;

        TimeManager.Instance.UpdateUI();
    }

    public void LoadStamina()
    {
        if (!System.IO.File.Exists(baseDataPath))
        {
            Debug.LogWarning("baseData.json 파일이 존재하지 않습니다.");
            return;
        }

        string json = System.IO.File.ReadAllText(baseDataPath);
        GameBaseData baseData = JsonUtility.FromJson<GameBaseData>(json);

        StaminaManager.Instance.staminaStates = baseData.staminaStates;
        StaminaManager.Instance.UpdateStaminaUI();
    }

    public void LoadMoney()
    {
        if (!System.IO.File.Exists(baseDataPath))
        {
            Debug.LogWarning("baseData.json 파일이 존재하지 않습니다.");
            return;
        }

        string json = System.IO.File.ReadAllText(baseDataPath);
        GameBaseData baseData = JsonUtility.FromJson<GameBaseData>(json);

        InventoryManager.Instance.PlayerMoney = baseData.money;
        //BaseUI.Instance.SetMoneyText(InventoryManager.Instance.playerMoney.ToString());
    }

    public void SaveInventory()
    {
        InventoryData data = new InventoryData();

        for (int i = 0; i < InventoryManager.Instance.slots.Count; i++)
        {
            var slot = InventoryManager.Instance.slots[i];
            if (slot.itemData != null)
            {
                data.savedInventory.Add(new savedInventroyItem
                {
                    slotIndex = i,
                    itemName = slot.itemData.itemName,
                    quantity = slot.quantity
                });
            }
        }

        string json = JsonUtility.ToJson(data, true);
        System.IO.File.WriteAllText(inventroyPath, json);
        Debug.Log("인벤토리 정보가 저장되었습니다. " + inventroyPath);
    }

    public void LoadInventory()
    {
        if (!System.IO.File.Exists(inventroyPath))
        {
            Debug.LogWarning("inventory.json 파일이 존재하지 않습니다." + farmPath);
            return;
        }

        string json = System.IO.File.ReadAllText(inventroyPath);
        InventoryData data = JsonUtility.FromJson<InventoryData>(json);

        foreach (var slot in InventoryManager.Instance.slots)
        {
            slot.itemData = null;
            slot.quantity = 0;
        }

        foreach (var savedItem in data.savedInventory)
        {
            if (savedItem.slotIndex >= 0 && savedItem.slotIndex < InventoryManager.Instance.slots.Count)
            {
                var slot = InventoryManager.Instance.slots[savedItem.slotIndex];
                slot.itemData = ItemManager.Instance.GetItemByName(savedItem.itemName);
                slot.quantity = savedItem.quantity;
            }
        }

        UIManager.Instance.UpdateInventoryAndQuickSlot();
    }

    public void SaveStats()
    {
        StatsData data = new StatsData();

        foreach (var stat in DaughterManager.Instance.GetStats())
        {
            data.savedStats.Add(new SavedStat
            {
                statType = stat.statType,
                currentValue = stat.CurrentValue,
                maxValue = stat.MaxValue
            });
        }

        string json = JsonUtility.ToJson(data, true);
        System.IO.File.WriteAllText(statsPath, json);
        Debug.Log("스탯 정보가 저장되었습니다. " + statsPath);
    }

    public void LoadStats()
    {
        if (!System.IO.File.Exists(statsPath))
        {
            Debug.LogWarning("stats.json 파일이 존재하지 않습니다.");
            return;
        }

        string json = System.IO.File.ReadAllText(statsPath);
        StatsData data = JsonUtility.FromJson<StatsData>(json);

        foreach (var savedStat in data.savedStats)
        {
            DaughterManager.Instance.SetStats(savedStat.statType, (int)savedStat.currentValue);
        }
    }

    private IEnumerator AutoSaveRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(autoSaveInterval);
            Save();
        }
    }

    public void Save()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        if (currentScene.name.Contains("TownScene"))
        {
            SaveFarm();
        }
        if (currentScene.name.Contains("HomeScene"))
        {
            SaveStats();
        }
        SaveBase();
        SaveInventory();

        Debug.Log("💾 저장 완료");
    }

}
