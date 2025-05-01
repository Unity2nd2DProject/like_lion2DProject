using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChangeDetector : Singleton<SceneChangeDetector>
{
    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.sceneUnloaded += OnSceneUnloaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        SceneManager.sceneUnloaded -= OnSceneUnloaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log($"{scene.name} is loaded...");

        if (scene.name.Contains("TownScene"))
        {
            TimeManager.Instance.ResumeTime();
            StartCoroutine(DelayedUpdateUI(scene));
            
        }
        else if (scene.name.Contains("HomeScene"))
        {
            TimeManager.Instance.PauseTime();
            StartCoroutine(DelayedUpdateUI(scene));
        }
    }

    private IEnumerator DelayedUpdateUI(Scene scene) // 필수!
    {
        yield return null; // 한 프레임 대기
        if (TimeImageUI.Instance != null && TimeImageUI.Instance.timeImage != null)
        {
            TimeManager.Instance.UpdateUI();
            StaminaManager.Instance.UpdateStamina();

            if (scene.name.Contains("TownScene"))
            {
                SaveManager.Instance.LoadFarm();
            }
        }
        else
        {
            
        }
    }

    public void OnSceneUnloaded(Scene scene)
    {
        //Debug.Log($"{scene.name} is unloaded...");
    }
}
