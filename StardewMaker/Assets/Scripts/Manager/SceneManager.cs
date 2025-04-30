using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManager : Singleton<SceneManager>
{
    void OnEnable()
    {
        UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        UnityEngine.SceneManagement.SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log($"{scene.name}({scene}) is loaded...");

        //if (scene.name == "TownScene-yh")
        //{
        //    TimeManager.Instance.LoadLight();
        //}
    }

    void OnActiveSceneChanged(Scene previousScene, Scene newScene)
    {
        Debug.Log($"{previousScene.name} -> {newScene.name}");
    }
}
