using System;
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
    public bool isDebug = false;

    private GameState currentState;
    public static event Action<GameState> OnGameStateChanged;

    [HideInInspector]
    public GameMode currentMode;

    public string arrivalPointName { get; set; } // 씬 전환 시 캐릭터의 위치를 잡아주기 위함

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
        if(sceneName == "TownScene")
        {
            currentMode = GameMode.TOWN;
        }
        else if (sceneName == "HomeScene")
        {
            currentMode = GameMode.HOME;
        }

        // 테스트용
        if(sceneName == "TownHyunkyu")
        {
            currentMode = GameMode.TOWN;
        }
        else if (sceneName == "HomeHyunkyu")
        {
            currentMode = GameMode.HOME;
        }

        SceneManager.LoadScene(sceneName);

        
        // todo 데이터 저장
    }
}
