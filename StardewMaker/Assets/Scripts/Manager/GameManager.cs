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

    protected override void Awake()
    {
        base.Awake();
    }

    public void NextDay()
    {
        Debug.Log("☀️ ============ NextDay.. ==============");
        CropManager.Instance.NextDay();
        FarmLandManager.Instance.NextDay();
        TreeManager.Instance.NextDay();
    }

    public void SetGameState(string tag, GameState newState)
    {
        currentState = newState;
        if (isDebug) Debug.Log($"From {tag} GameState changed to: {newState}");
        OnGameStateChanged?.Invoke(newState);
    }

    public void changeScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
        // todo 데이터 저장
    }
}
