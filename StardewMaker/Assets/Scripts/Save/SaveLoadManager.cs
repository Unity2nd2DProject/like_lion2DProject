using System.Collections;
using UnityEngine;

public class SaveLoadManager : Singleton<SaveLoadManager>
{

    // C:\Users\<사용자 이름>\AppData\LocalLow\<회사 이름>\<제품 이름>
    private string savePath;

    public SaveData currentSaveData;

    // 저장 관련 가장 빨리 실행되어야 하는곳 다른 로드 하는곳들보다 빠르게 로드 되어야 함. 
    protected override void Awake() 
    {
        base.Awake();

        savePath = Application.persistentDataPath + "/saveData.json";

        // Load();
    }

    public void SaveGame()
    {
        currentSaveData = new SaveData();
        currentSaveData.farmSaveData = FarmingManager.Instance.GetFarmingData();

        // 농장 데이터

        // 인벤토리 데이터

        // 플레이어 데이터

        // 시간 데이터? 플레이어 데이터와 같나

        // 퀘스트 스토리 진행상황 데이터



        // currentSaveData.savedTimeData = TimeManager.Instance.GetCurrentTimeData();
        string saveDataToSave = JsonUtility.ToJson(currentSaveData);
        System.IO.File.WriteAllText(savePath, saveDataToSave);
    }

}
