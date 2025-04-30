using UnityEngine;

public class SaveManager : Singleton<SaveManager>
{
    private string path => Application.persistentDataPath + "/save.json";

    public void SaveFarm()
    {
        GameData data = new GameData
        {
            savedFarmLands = FarmLandManager.Instance.SaveFarmLands(),
            savedCrops = CropManager.Instance.SaveCrops(),
            savedTrees = TreeManager.Instance.SaveTrees(),
        };

        string json = JsonUtility.ToJson(data, true);
        System.IO.File.WriteAllText(path, json);
        Debug.Log("Saved in " + path);
    }

    public void LoadFarm()
    {
        if (!System.IO.File.Exists(path))
        {
            return;
        }

        string json = System.IO.File.ReadAllText(path);
        GameData data = JsonUtility.FromJson<GameData>(json);

        FarmLandManager.Instance.LoadFarmLands(data.savedFarmLands);
        CropManager.Instance.LoadCrops(data.savedCrops);
        TreeManager.Instance.LoadTrees(data.savedTrees);
    }
}
