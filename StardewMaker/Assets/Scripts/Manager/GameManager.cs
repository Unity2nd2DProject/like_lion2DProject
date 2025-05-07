using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameState
{
    UI,
    PLAYING
}

public enum GameMode
{
    HOME,
    TOWN,
}

public class GameManager : Singleton<GameManager>
{
    private string TAG = "[GameManager]";
    public bool isDebug = true;

    private GameState currentState;
    public static event Action<GameState> OnGameStateChanged;

    [HideInInspector]
    public GameMode currentMode;

    public string arrivalPointName { get; set; } // 씬 전환 시 캐릭터의 위치를 잡아주기 위함

    public Dialog wantedDialog; // 딸이 원하는 스케줄이 있는 대화 임시 저장
    public List<ScheduleType> actualScheuleType = new(); // 실제 진행할 스케줄

    protected override void Awake()
    {
        base.Awake();
    }

    public void SetGameState(string tag, GameState newState)
    {
        currentState = newState;
        if (isDebug) Debug.Log($"From {tag} GameState changed to: {newState}");
        OnGameStateChanged?.Invoke(newState);
    }

    public void changeScene(string sceneName)
    {
        if (sceneName.Contains("Town"))
        {
            currentMode = GameMode.TOWN;
        }
        else if (sceneName.Contains("Home"))
        {
            currentMode = GameMode.HOME;
        }

        SceneManager.LoadScene(sceneName);

        // todo 데이터 저장
        if (sceneName.Contains("HomeScene"))
        {
            SaveManager.Instance.SaveFarm();
        }
    }
}
