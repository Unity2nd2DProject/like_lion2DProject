using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameState
{
    UI,
    PLAYING
}

public class GameManager : Singleton<GameManager>
{
    private string TAG = "[GameManager]";
    public bool isDebug = false;

    private GameState currentState;
    public static event Action<GameState> OnGameStateChanged;

    public GameObject inventory;

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
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
        // todo 데이터 저장
    }
}
