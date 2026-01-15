using System.Collections;
using UnityEngine;

public class SaveLoadManager : Singleton<SaveLoadManager>
{

    // C:\Users\<사용자 이름>\AppData\LocalLow\<회사 이름>\<제품 이름>


    private string savePath => Application.persistentDataPath + "/Save/save.json";

    public SaveData currentSaveData = new SaveData();



    // 저장 관련 가장 빨리 실행되어야 하는곳 다른 로드 하는곳들보다 빠르게 로드 되어야 함. 
    protected override void Awake() 
    {
        base.Awake();

        // Load();
    }

}
