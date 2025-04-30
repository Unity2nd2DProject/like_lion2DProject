using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChangeDetector : Singleton<SceneChangeDetector>
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

        if (scene.name == "TownScene-yh")
        {
            TimeManager.Instance.ResumeTime();
            StartCoroutine(DelayedUpdateUI());
        }
        else if (scene.name == "HomeScene-yh")
        {
            TimeManager.Instance.PauseTime();
            StartCoroutine(DelayedUpdateUI());
        }
    }

    private IEnumerator DelayedUpdateUI() // 필수!
    {
        yield return null; // 한 프레임 대기
        if (TimeImageUI.Instance != null && TimeImageUI.Instance.timeImage != null)
        {
            TimeManager.Instance.UpdateUI();
            StaminaManager.Instance.UpdateStamina();
        }
        else
        {
            Debug.LogWarning("TimeImageUI is not initialized yet.");
        }
    }

    void OnActiveSceneChanged(Scene previousScene, Scene newScene)
    {
        Debug.Log($"{previousScene.name} -> {newScene.name}");
    }
}
