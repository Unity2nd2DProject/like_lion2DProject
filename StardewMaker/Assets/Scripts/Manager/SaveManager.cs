using System.Collections;
using UnityEngine;

public class SaveManager : Singleton<SaveManager>
{
    public float autoSaveInterval = 10f;

    private string farmPath => Application.dataPath + "/Save/farm.json";
    private string timePath => Application.dataPath + "/Save/time.json";

    private void Start()
    {
        StartCoroutine(AutoSaveRoutine());
    }

    public void SaveFarm()
    {
        GameData data = new GameData
        {
            savedFarmLands = FarmLandManager.Instance.SaveFarmLands(),
            savedCrops = CropManager.Instance.SaveCrops(),
            savedTrees = TreeManager.Instance.SaveTrees(),
        };

        string json = JsonUtility.ToJson(data, true);
        System.IO.File.WriteAllText(farmPath, json);
        Debug.Log("농장 정보가 저장되었습니다. " + farmPath);
    }

    public void LoadFarm()
    {
        if (!System.IO.File.Exists(farmPath))
        {
            Debug.LogWarning("save.json 파일이 존재하지 않습니다.");
            return;
        }

        string json = System.IO.File.ReadAllText(farmPath);
        GameData data = JsonUtility.FromJson<GameData>(json);

        FarmLandManager.Instance.LoadFarmLands(data.savedFarmLands);
        CropManager.Instance.LoadCrops(data.savedCrops);
        TreeManager.Instance.LoadTrees(data.savedTrees);
    }

    public void SaveTime()
    {
        GameDateTime dateTime = new GameDateTime
        {
            year = TimeManager.Instance.currentYear,
            season = (int)TimeManager.Instance.currentSeason,
            day = TimeManager.Instance.currentDay,
            hour = TimeManager.Instance.currentHour,
            minute = TimeManager.Instance.currentMinute
        };

        string json = JsonUtility.ToJson(dateTime, true);
        System.IO.File.WriteAllText(timePath, json);
        Debug.Log("시간 정보가 저장되었습니다. " + timePath);
    }


    public void LoadTime()
    {

        if (!System.IO.File.Exists(timePath))
        {
            Debug.LogWarning("time.json 파일이 존재하지 않습니다.");
            return;
        }

        string json = System.IO.File.ReadAllText(timePath);
        GameDateTime dateTime = JsonUtility.FromJson<GameDateTime>(json);

        TimeManager.Instance.currentYear = dateTime.year;
        TimeManager.Instance.currentSeason = (Season)dateTime.season;
        TimeManager.Instance.currentDay = dateTime.day;
        TimeManager.Instance.currentHour = dateTime.hour;
        TimeManager.Instance.currentMinute = dateTime.minute;

        TimeManager.Instance.UpdateUI();
    }

    private IEnumerator AutoSaveRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(autoSaveInterval);
            SaveManager.Instance.SaveFarm(); // 농장 데이터
            SaveManager.Instance.SaveTime(); // 시간 데이터
            Debug.Log("💾 자동 저장 완료");
        }
    }
}
